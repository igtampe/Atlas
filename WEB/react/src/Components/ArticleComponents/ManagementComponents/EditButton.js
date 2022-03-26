import { Edit } from '@mui/icons-material';
import { IconButton, Tooltip } from '@mui/material';
import React from 'react';
import { useHistory } from "react-router-dom";

export function EditButton(props) {
    const history = useHistory();

    const launchEditor= () => { 
        history.push("/EditArticle/" + props.title) 
    }

    let editLevel = props.User ? props.User.editLevel : 0
    let isAdmin = props.User ? props.User.isAdmin : false
    if(editLevel < props.level && !isAdmin){return(<></>)}

    return(<Tooltip title={'Edit ' + props.title}><IconButton onClick={launchEditor}><Edit/></IconButton></Tooltip>)
}