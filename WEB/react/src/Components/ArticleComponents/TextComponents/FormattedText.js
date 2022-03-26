import { Link } from '@mui/material';
import React from 'react';

export function ParseFormattedText(FT) {

    var ReturnElement = <>{FT.text.replace("\\n","\r\n")}</>;
    
    if(FT.code){
        ReturnElement = <code>{ReturnElement}</code>
    }

    if(Boolean(FT.link)){
        var link = FT.link.toLowerCase().startsWith("https://") || FT.link.toLowerCase().startsWith("http://") ? FT.link : "/Article/" + FT.link
        ReturnElement = <Link color='secondary' href={link}>{ReturnElement}</Link>
    }
    
    if(FT.bold){ReturnElement = <b>{ReturnElement}</b>}
    if(FT.italic){ReturnElement = <i>{ReturnElement}</i>}
    if(FT.underline){ReturnElement = <u>{ReturnElement}</u>}

    return ReturnElement;
}

export function ParseFormattedTextList(FTList){ 
    return(<> {FTList.map(ft=>ParseFormattedText(ft))}</>) 
}