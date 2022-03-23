import * as React from 'react';
import { Grid, Typography, Container } from '@mui/material';
import SearchField from './Reusable/SearchField';

export default function Home(props) {

    return (<>

        <Typography>
            <Grid container spacing={0} direction="column" alignItems="center" justifyContent="center" style={{ minHeight: '50vh' }}>
                <Grid item xs={12}>
                    <Container style={props.DarkMode ? { backgroundColor: '#444444', padding: '50px' } : { backgroundColor: '#ebebeb', padding: '50px' }}>
                        <Typography>
                            <Typography variant="h6" style={{ textAlign: "center", marginBottom:'10px' }}> <img src="Logo.png" alt="Logo" width="300px" /> </Typography>
                            <SearchField/>
                        </Typography>
                    </Container>
                </Grid>
            </Grid>
        </Typography>

    </>);
}