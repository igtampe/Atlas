import { Grid } from '@mui/material';
import React from 'react';
import { ParseImage } from './ImageBox';

export function ParseImageGrid(ImgGrid) {

    return(
        <>
            <div style={{textAlign:'center'}}><b>{ImgGrid.title}</b></div>
            <Grid container spacing={2}> {ImgGrid.images.map(A=>ParseImageGridItem(A))}</Grid>
        </>
        
    )

}

function ParseImageGridItem(GItem){

    console.log(GItem);

    return(
        <Grid item xs={GItem.width}>
            {ParseImage(GItem)}
        </Grid>
    )

}