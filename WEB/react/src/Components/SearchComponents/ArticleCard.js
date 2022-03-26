import { Card, CardContent, Divider, Link } from '@mui/material';
import React from 'react';
import { ParseImage } from '../ArticleComponents/TextComponents/ImageBox';
import { ParseParagraph } from '../ArticleComponents/TextComponents/Paragraph';

export default function ArticleCard(Article) {

    return (<>
        <Card style={{ padding: '10px' }}>
            <CardContent>
                <table>
                    <tr>
                        <td width='250px' style={{textAlign:'center'}}>
                            {GetFirstSidebarImage(Article.sidebar)}
                        </td>
                        <td>
                            <Link color='secondary' href={'/Article/' + Article.title}><div style={{ fontSize: 35 }}>{Article.title}</div></Link>
                            <Divider /> <br />
                            <div style={{ fontSize: '10px' }}>
                                <i> Last updated on {(new Date(Article.dateUpdated)).toDateString()} by {Article.lastAuthor}.
                                    Created {(new Date(Article.dateCreated)).toDateString()} by {Article.originalAuthor}.<br /><br /></i>
                            </div>
                            {GetFirstBodyParagraph(Article.components)}
                        </td>
                    </tr>
                </table>
            </CardContent>
        </Card>
    </>);

}

function GetFirstBodyParagraph(elements) {
    if (elements) {
        //Find the first paragraph
        for (let index = 0; index < elements.length; index++) {
            const e = elements[index];
            if (e.componentName === "PARAGRAPH") {
                return (ParseParagraph(e));
            }
        }
    }

    //props.elements is our thing
    return (<i>No paragraphs were found in this article</i>);
}

function GetFirstSidebarImage(elements) {
    if (elements) {
        //Find the first paragraph
        for (let index = 0; index < elements.length; index++) {
            const e = elements[index];
            if (e.componentName === "PARAGRAPH") {
                if (e.image) { return ( <ArticleImage imageurl={e.image.imageURL} alt={e.image.alt}/> )}
            }
        }
    }

    //props.elements is our thing
    return ( <ArticleImage imageurl={'icons/article.png'} alt={'Article'}/> )
}

function ArticleImage(props){
    return (<a href={props.imageurl}>
        <img src={props.imageurl} alt={props.alt} width='75%' height='75%' style={{ objectFit: 'cover' }} />
    </a>)
}