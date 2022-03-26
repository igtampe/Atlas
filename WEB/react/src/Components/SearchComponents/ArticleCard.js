import { Card, CardContent, CircularProgress, Divider, Link } from '@mui/material';
import React from 'react';
import { ParseParagraph } from '../ArticleComponents/TextComponents/Paragraph';

export default function ArticleCard(Article) {

    return (<>
        <Card style={{ padding: '10px' }}>
            <CardContent>
                <Link color='secondary' href={'/Article/' + Article.title}><div style={{ fontSize: 35 }}>{Article.title}</div></Link>
                <Divider /> <br />
                <div style={{ fontSize: '10px' }}>
                    <i> Last updated on {(new Date(Article.dateUpdated)).toDateString()} by {Article.lastAuthor}.
                        Created {(new Date(Article.dateCreated)).toDateString()} by {Article.originalAuthor}.<br /><br /></i>
                </div>
                {ParseElements(Article.components)}
            </CardContent>
        </Card>
    </>);

}

function ParseElements(elements) {
    //Find the first paragraph
    for (let index = 0; index < elements.length; index++) {
        const e = elements[index];
        if(e.componentName === "PARAGRAPH" ){
            return (ParseParagraph(e));
        }
    }

    //props.elements is our thing
    return (<i>No paragraphs were found in this article</i>);
}