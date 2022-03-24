import { Card, CardContent, CircularProgress, Divider } from '@mui/material';
import React, { useState } from 'react';
import { GetArticle } from '../API/Article';
import { ParseImageGrid } from './ArticleComponents/ImageGrid';
import { ParseList } from './ArticleComponents/List';
import { ParseParagraph } from './ArticleComponents/Paragraph';
import { ParseSection } from './ArticleComponents/Section';
import { ParseTable } from './ArticleComponents/Table';

export default function Article(props) {

    const [Article, setArticle] = useState(undefined)
    const [Loading, setLoading] = useState(false);
    const [Error, setError] = useState(undefined)

    if (!Article && !Loading) { GetArticle(setLoading, props.title, setArticle, setError) }

    if (!Article && !Error) {
        return (<>

            <div style={{ textAlign: 'center', marginTop: '50px', marginBottom: '50px' }}>
                <CircularProgress /><br /><br />Loading {props.title}
            </div>

        </>)
    }

    if (Error) {
        return (<>
            <h1>Oops</h1>
            <Divider /> <br />
            {Error}
        </>);
    }

    document.title = "The UMS Wiki - " + Article.title;

    return (<>
        <div style={{ fontSize: 35 }}>{Article.title}</div>

        <Divider /> <br />
        {Article.sidebar
            ? <Sidebar elements={Article.sidebar} />
            : <></>
        }

        <div style={{ fontSize: '10px' }}>
            <i> Last updated on {(new Date(Article.dateUpdated)).toDateString()} by {Article.lastAuthor}.
                Created {(new Date(Article.dateCreated)).toDateString()} by {Article.originalAuthor}.<br /><br /></i>
        </div>
        {ParseElements(Article.components)}
    </>);

}

function Sidebar(props) {

    return (<>
        <div className='square' style={{ float: 'right', maxWidth: '300px' }}>
            <Card style={{ padding: '10px' }}>
                <CardContent style={{ textAlign: 'center' }}>
                    {ParseElements(props.elements)}
                </CardContent>
            </Card>
        </div>
    </>);
}

function ParseElements(elements) {

    //props.elements is our thing
    return (<>{elements.map(e => ParseElement(e))}</>)

}

function ParseElement(e) {

    if (!e) { return <></> }
    switch (e.componentName) {
        case "TABLE":
            return (ParseTable(e))
        case "IMAGE_GRID":
            return (ParseImageGrid(e))
        case "LIST":
            return (ParseList(e))
        case "PARAGRAPH":
            return (ParseParagraph(e))
        case "SECTION":
            return (ParseSection(e))
        default:
            return (<p>{JSON.stringify(e)}</p>);
    }

}