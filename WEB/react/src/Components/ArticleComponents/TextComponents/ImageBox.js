import { Card, CardContent } from '@mui/material';
import React from 'react';
import { APIURL } from '../../../API/common';

export function ParseImage(Image) {

    let src = Image.imageURL;
    let apiImageID = undefined

    if (!src.startsWith("ht")) {
        src = APIURL + "/API/Images/" + src
        apiImageID = Image.imageURL;
    }

    return <Card style={{ margin: '20px' }}>
        <CardContent>
            <table style={{ width: '100%' }}>
                <tr> <td>
                    {!apiImageID ? <a href={Image.imageURL}><img src={src} alt={Image.altText} width='100%' /></a>
                        : <a href={'/Image/' + apiImageID}><img src={src} alt={Image.altText} width='100%' /></a>}
                </td> </tr>
                <tr> <td> {Image.description} </td> </tr>
            </table>
        </CardContent>
    </Card>

}