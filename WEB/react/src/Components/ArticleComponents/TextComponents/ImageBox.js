import { Card, CardContent } from '@mui/material';
import React from 'react';

export function ParseImage(Image) {

    return <Card style={{margin:'20px'}}>
        <CardContent>
            <table style={{width:'100%'}}>
                <tr>
                    <td>
                        <a href={Image.imageURL}><img src={Image.imageURL} alt={Image.altText} width='100%'/></a>
                    </td>
                </tr>
                <tr>
                    <td>
                        {Image.description}
                    </td>
                </tr>
            </table>
        </CardContent>
    </Card>

}