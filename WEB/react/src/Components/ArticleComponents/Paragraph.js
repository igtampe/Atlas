import React from 'react';
import { ParseFormattedTextList } from './FormattedText';
import { ParseImage } from './ImageBox';

export function ParseParagraph(Paragraph) {

    //If there's an image add the image on the side:
    var ImageBox = <></>

    if(Boolean(Paragraph.image)){

        var Square = false;
        var Style = {};
        switch (Paragraph.image.position) {
            case 1:
                Square = true;
                Style={ float:'left', maxWidth:'200px' }
                break;
            case 2:
                Square = true;
                Style={ float:'right', maxWidth:'200px' }
                break;
            default:
                break;
        }

        //We have an image
        ImageBox = <div className={Square ? 'square' : ''} style={{...Style}} >{ParseImage(Paragraph.image,Square,Style)}</div> 

    }

    return <>
        {ImageBox}
        <p> {ParseFormattedTextList(Paragraph.text)} </p>
    </>


}