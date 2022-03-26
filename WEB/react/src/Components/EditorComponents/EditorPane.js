import { Edit } from '@mui/icons-material';
import { IconButton, TextField, Tooltip } from '@mui/material';
import React from 'react';
import { useHistory } from "react-router-dom";

export function EditorPane(props) {
    
    return(<>
    
    <TextField value={props.text} onChange={(e)=>props.setText(e.target.value)} fullWidth multiline minRows={20} inputProps={{style:{fontFamily:'monospace'}}}/>

    </>)
}