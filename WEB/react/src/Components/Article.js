import { Card, CardContent, CircularProgress, Divider, Link } from '@mui/material';
import React, { useState } from 'react';
import { GetArticle } from '../API/Article';
import { DeleteBButton } from './ArticleComponents/ManagementComponents/DeleteButton';
import { EditButton } from './ArticleComponents/ManagementComponents/EditButton';
import { LockButton } from './ArticleComponents/ManagementComponents/LockButton';
import { ParseImageGrid } from './ArticleComponents/TextComponents/ImageGrid';
import { ParseList } from './ArticleComponents/TextComponents/List';
import { ParseParagraph } from './ArticleComponents/TextComponents/Paragraph';
import { ParseSection } from './ArticleComponents/TextComponents/Section';
import { ParseTable } from './ArticleComponents/TextComponents/Table';

export default function Article(props) {

    const [Article, setArticle] = useState(undefined)
    const [Loading, setLoading] = useState(false);
    const [Error, setError] = useState(undefined)

    if (!Article && !Loading && Boolean(props.title)) { GetArticle(setLoading, props.title, setArticle, setError) }

    return(<ArticleDisplay {...props} Article={Article} Error={Error}/>)

}

export function ArticleDisplay(props) {

    if (!props.Article && !props.Error) {
        return (<>

            <div style={{ textAlign: 'center', marginTop: '50px', marginBottom: '50px' }}>
                <CircularProgress /><br /><br />Loading {props.title}
            </div>

        </>)
    }

    if (props.Error) {
        return (<>
            <h1>Oops</h1>
            <Divider /> <br />
            {props.Error === "Article was not found!" ? <>
                An article called "{props.title}" was not found. You can <Link color='secondary' href={'/NewArticle?title='+props.title}>create it</Link> if you'd like.
            </>
            : props.Error}
        </>);
    }

    document.title = "The UMS Wiki - " + props.Article.title;

    return (<>
        <table width='100%'>
            <tr>
                <td><div style={{ fontSize: 35 }}>{props.Article.title}</div></td>
                {
                    props.noManage ? <></> : <>
                        <td width={1}><EditButton {...props} level={props.Article.editLevel} /></td>
                        <td width={1}><LockButton {...props} level={props.Article.editLevel} /></td>
                        <td width={1}><DeleteBButton {...props} /></td>
                    </>
                }
            </tr>
        </table>
        <Divider /> <br />
        {props.Article.sidebar
            ? <Sidebar elements={props.Article.sidebar} Vertical={props.Vertical} />
            : <></>
        }

        <div style={{ fontSize: '10px' }}>
            <i> Last updated on {(new Date(props.Article.dateUpdated)).toDateString()} by {props.Article.lastAuthor}.
                Created {(new Date(props.Article.dateCreated)).toDateString()} by {props.Article.originalAuthor}.<br /><br /></i>
        </div>
        {ParseElements(props.Article.components, props.Vertical)}
    </>);


}

function Sidebar(props, Vertical) {

    return (<>
        <div className='square' style={{ float: 'right', maxWidth: '300px' }}>
            <Card style={{ padding: '10px' }}>
                <CardContent style={{ textAlign: 'center' }}>
                    {ParseElements(props.elements, Vertical)}
                </CardContent>
            </Card>
        </div>
    </>);
}

function ParseElements(elements, Vertical) {

    //props.elements is our thing
    return (<>{elements.map(e => ParseElement(e, Vertical))}</>)

}

function ParseElement(e, Vertical) {

    if (!e) { return <></> }
    switch (e.componentName) {
        case "TABLE":
            return (ParseTable(e))
        case "IMAGE_GRID":
            return (ParseImageGrid(e, Vertical))
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