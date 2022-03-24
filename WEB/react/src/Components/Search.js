import { CircularProgress, Divider } from '@mui/material';
import React, { useState } from 'react';
import { GetArticles } from '../API/Article';
import ArticleCard from './SearchComponents/ArticleCard';

export function SearchComponent(props) {

    const [results,setResults] = useState(undefined);
    const [loading,setLoading] = useState(undefined);
    
    const [noMas, setNoMas] = useState(false);

    const [displayed, setDisplayed] = useState(0);

    const setArticles = (Articles) => {

        setDisplayed(displayed+Articles.length);
        setResults(Articles)

        //No more articles exist if there's no articles displayed, or if the lenght of articles is not 20
        setNoMas(displayed+Articles.length === 0 || Articles.length !== 20);

    }

    if(!results && !loading) { GetArticles(setLoading,setArticles,props.query,displayed,20); }

    if (!results) {
        return (<>
            <div style={{ textAlign: 'center', marginTop: '50px', marginBottom: '50px' }}>
                <CircularProgress/><br /><br />Searching for "{props.query}"
            </div>
        </>)
    }

    if (results.length===0) {
        return (<>
            <h1>Nada</h1>
            <Divider /> <br />
            Nothing was found!
        </>);
    }

    return(
        <>
            {results.map(a=>ArticleCard(a))}
        </>
    )

}