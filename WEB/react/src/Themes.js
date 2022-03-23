import { createTheme } from '@mui/material/styles';

//TODO: These are the Neco light and dark themes
//please replace these later.

export const lightTheme = createTheme({
    palette: {
      mode: 'light',
      primary: { main: '#E0E0E0', },
      secondary: { main: '#009800', },
    },
  });
  
  export const darkTheme = createTheme({
    palette: {
      mode: 'dark',
      primary: { main: '#6B6B6B', },
      secondary: { main: '#009800', },
    },
  })