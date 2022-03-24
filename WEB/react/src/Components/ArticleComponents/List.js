import React from 'react';
import { ParseFormattedTextList } from './FormattedText';

export function ParseList(ListText) {
    console.log(ListText.type)
    if(ListText.type === 45){
        return(<ul>
            {ListText.items.map(a=>ParseListItem(a))}    
        </ul>)
    } else {
        return(<ol>
            {ListText.items.map(a=>ParseListItem(a))}    
        </ol>)
    }
}

function ParseListItem(item){
    if(!item.sublist){return(<li>{ParseFormattedTextList(item.text)}</li>)}
    return(
        <li>{ParseFormattedTextList(item.text)}
            {ParseList(item.sublist)}
        </li>)
}