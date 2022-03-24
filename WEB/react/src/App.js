import React, { useState } from 'react';
import { Route, Redirect, useLocation, useParams } from 'react-router';
import { ThemeProvider } from '@mui/material/styles';
import Cookies from 'universal-cookie/es6';
import useWindowDimensions from './Components/Hooks/useWindowDimensions';
import NavMenu from './Components/Navbar/NavBar';

import { GetMe } from './API/User';
import { darkTheme, lightTheme } from './Themes';
import { CircularProgress, CssBaseline, Container } from '@mui/material';
import { Footer } from './Components/Footer';
import Home from './Components/Home';
import Auth from './Components/Auth';
import Article from './Components/Article';
import { SearchComponent } from './Components/Search';

//Cookies should only really be accessed here.
const cookies = new Cookies();

function CenteredCircular() { return (<div style={{ textAlign: 'center' }}> <CircularProgress /> </div>) }

function useQuery() {
  const { search } = useLocation();

  return React.useMemo(() => new URLSearchParams(search), [search]);
}

export default function App() {

  //Width of the window. Used to determine if we need to switch to a vertical arrangement
  const { width } = useWindowDimensions();
  const Vertical = width < 900;

  //Auth stuff. Session and User is passed down to the components.
  const [Session, setSession] = useState(undefined)
  const [User, setUser] = useState(undefined)

  //Loading usestate to make sure we don't start loading 50 times
  const [Loading, setLoading] = useState(false)

  //Warning to show a dialogbox to say ```y o    s i g n    o u t```
  const [InvalidSession, setInvalidSession] = useState(false);

  //Dark mode will not be a user saved preference. It'll be saved in a cookie
  const [darkMode, setDarkMode] = useState(undefined);

  //This is the set session that must be passed down
  const SetSession = (SessionID) => {

    //Set the cookie
    cookies.set("SessionID", SessionID)

    //set the usestates
    setSession(SessionID)
    setInvalidSession(false)
  }

  const ToggleDarkMode = () => {
    //What is !undefined? Please tell me it's "true" just to make my life easier.
    if (darkMode) {
      //Delete the cookie
      cookies.remove("DarkMode", { path: '/' })
    } else {
      //Add the cookie
      cookies.set("DarkMode", true, { path: '/' })
    }

    setDarkMode(!darkMode);

  }

  //Assuming there's a valid session, this will automatically trigger a refresh
  const RefreshUser = () => { setUser(undefined); }

  //This runs at legitiately *EVERY* time we load and render ANY page in the app
  //So here we can set the session and user

  //Check that session reflects the cookie's state
  if (Session !== cookies.get("SessionID")) { setSession(cookies.get("SessionID")) }
  if (darkMode !== cookies.get("DarkMode")) { setDarkMode(cookies.get("DarkMode")) }

  //Check that the user is defined
  if (Session && !InvalidSession && !Loading && !User) {
    //If there is a session, and it's not invalid, and
    //we're not already loading a user, and the user is not set

    //Well, time to get the user
    GetMe(setLoading, Session, setUser, setInvalidSession)
  }

  let query = useQuery();
  // <Layout DarkMode={darkMode} ToggleDarkMode={ToggleDarkMode} Session={Session} InvalidSession={InvalidSession} setSession = {SetSession} RefreshUser = {RefreshUser} User={User} Vertical={Vertical}>
  return (
    <ThemeProvider theme={darkMode ? darkTheme : lightTheme}>
      <CssBaseline />
      <Layout DarkMode={darkMode} ToggleDarkMode={ToggleDarkMode} Session={Session} InvalidSession={InvalidSession} setSession={SetSession} RefreshUser={RefreshUser} User={User} Vertical={Vertical}>
      <Route exact path='/'>
        <Home DarkMode={darkMode} Session={Session} InvalidSession={InvalidSession} setSession={SetSession} RefreshUser={RefreshUser} User={User} Vertical={Vertical} />
      </Route>
      <Route path='/Login'>
        {Session ? <Redirect to='/' /> : <Auth DarkMode={darkMode} />}
      </Route>
      <Route path='/Admin'>
        {Session
          ? <> {User
            ? <> {User.isAdmin
              ? <>Admin component here</>
              : <>You do not have permission to access this resource</>} </>
            : <CenteredCircular />} </>
          : <Redirect to='/Login' />}
      </Route>
      <Route path='/Search'>
        <SearchComponent query={query.get("query")}/>
      </Route>
      <Route path='/NewArticle' children={
        <PreArticleDisplay newArticle={true} DarkMode={darkMode} Session={Session} User={User} Vertical={Vertical} />
      } />
      <Route path='/Article/:title' children={
        <PreArticleDisplay DarkMode={darkMode} Session={Session} User={User} Vertical={Vertical} />
      } />
      <Footer />
      </Layout>
    </ThemeProvider>
  );
}

function PreArticleDisplay(props) {
  let { title } = useParams();
  return (<Article {...props} title={title} />)
}

export function Layout(props) {
  return (
    <div>
      <NavMenu hidden={props.hidden} DarkMode={props.DarkMode} ToggleDarkMode={props.ToggleDarkMode} Session={props.Session} InvalidSession={props.InvalidSession}
        setSession={props.SetSession} RefreshUser={props.RefreshUser} User={props.User} />
      <Container maxWidth='xl'> {props.children} </Container>
    </div>
  );
}
