import { Delete, Edit, Lock } from '@mui/icons-material';
import { Card, CardContent, CircularProgress, Divider, IconButton, Tooltip } from '@mui/material';
import React, { useState } from 'react';
import { GetArticle } from '../API/Article';
import { ParseImageGrid } from './ArticleComponents/TextComponents/ImageGrid';
import { ParseList } from      './ArticleComponents/TextComponents/List';
import { ParseParagraph } from './ArticleComponents/TextComponents/Paragraph';
import { ParseSection } from   './ArticleComponents/TextComponents/Section';
import { ParseTable } from     './ArticleComponents/TextComponents/Table';

export default function Article(props) {

    const [Article, setArticle] = useState(undefined)
    const [Loading, setLoading] = useState(false);
    const [Error, setError] = useState(undefined)

    if (!Article && !Loading && Boolean(props.title)) { GetArticle(setLoading, props.title, setArticle, setError) }

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
        <table width='100%'>
            <tr>
                <td><div style={{ fontSize: 35 }}>{Article.title}</div></td>
                <td width={1}><Tooltip title='Edit the text of this article'><IconButton><Edit/></IconButton></Tooltip></td>
                <td width={1}><Tooltip title='Lock this Article'><IconButton><Lock/></IconButton></Tooltip></td>
                <td width={1}><Tooltip title='Delete this Article'><IconButton><Delete/></IconButton></Tooltip></td>
            </tr>
        </table>
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