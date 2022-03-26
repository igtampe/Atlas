import { Grid } from '@mui/material';
import React from 'react';
import { ParseImage } from './ImageBox';

export function ParseImageGrid(ImgGrid, Vertical) {

    return(
        <>
            <div style={{textAlign:'center'}}><b>{ImgGrid.title}</b></div>
            <Grid container spacing={2}> {ImgGrid.images.map(A=>ParseImageGridItem(A, Vertical))}</Grid>
        </>
    )

}

function ParseImageGridItem(GItem, Vertical){
    return(
        <Grid item xs={Vertical ? GItem.width * 2: GItem.width}>
            {ParseImage(GItem)}
        </Grid>
    )
}