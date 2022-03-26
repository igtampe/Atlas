import { Box, Tab, Tabs, Typography } from '@mui/material';
import React, { useState } from 'react';
import { EditorPane } from './EditorPane';
import { PreviewPane } from './PreviewPane';

//Maybe this should've  been a default component? strange....
function TabPanel(props) {
    const { children, value, index, ...other } = props;

    return (
        <div role="tabpanel" hidden={value !== index} id={`vertical-tabpanel-${index}`} aria-labelledby={`vertical-tab-${index}`} {...other} style={{ width: "100%" }}>
            <Box sx={{ p: 3 }}> <Typography>{children}</Typography> </Box>
        </div>
    );
}

function TabGroup(props) {
    
    return (
        <Tabs variant="fullWidth" value={props.value} onChange={props.handleChange} sx={{ borderRight: 1, borderColor: 'divider' }}>
            <Tab label={"Edit"} />
            <Tab disabled={!props.enablePreview} label={"Preview"} />
        </Tabs>

    )
}

export function MainPane(props) {
    
    const [value, setValue] = useState(0);
    const handleChange = (event, newValue) => { setValue(newValue); };
        
    return (
        <React.Fragment>
            <TabGroup orientation="horizontal" value={value} handleChange={handleChange} enablePreview={Boolean(props.title)}/>
            <Box sx={{ bgcolor: 'background.paper', display: 'flex' }} >
                <TabPanel value={value} index={0}>
                    <EditorPane {...props} open={value===0}/>
                </TabPanel>
                <TabPanel value={value} index={1}>
                    <PreviewPane {...props} open={value===1}/>
                </TabPanel>
           </Box>

        </React.Fragment>
    );

}