import { Divider, Typography } from '@mui/material';
import React from 'react';

export function Footer() {

    return(
      <>
        <Divider style={{marginTop:'25px', marginBottom:'25px'}}/>
        <Typography textAlign={'center'} color={'gray'} fontSize={'15px'} style={{marginBottom:'5x'}}>
              ©2022 Igtampe, No Rights Reserved. Powered by Atlas. 
        </Typography>
      </>
    )
  
}