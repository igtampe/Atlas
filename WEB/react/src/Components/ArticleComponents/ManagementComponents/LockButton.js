import { Lock } from '@mui/icons-material';
import { IconButton, Tooltip, Dialog, DialogActions,DialogContent,DialogContentText,DialogTitle, Button, TextField } from '@mui/material';
import React, { useState } from 'react';
import { UpdateArticleEditLevel } from '../../../API/Article';
import AlertSnackbar from '../../Reusable/AlertSnackbar';
import { useHistory } from "react-router-dom";

export function LockButton(props) {

    const history = useHistory();

    const [open,setOpen] = useState(false);
    const [newEditLevel, setNewEditLevel] = useState(props.level)
    
    const [inProgress, setInProgress] = useState(false);

    const [SnackOpen,setSnackOpen] = useState(false)
    const [result, setResult] = useState("")

    const onUpdate = () => { history.go(); }

    const onError = (reason) => {

        setResult({ severity: "error", text: reason })
        setSnackOpen(true)

    }

    const onOK = () => { UpdateArticleEditLevel(setInProgress,props.Session,props.title,newEditLevel,onUpdate, onError) }

    if(!props.User || (props.User.editLevel < props.level && !props.User.isAdmin)){return(<></>)}
    return(
        <>
            <Tooltip title='Lock this Article'><IconButton onClick={()=>setOpen(true)}><Lock/></IconButton></Tooltip>
            
            <Dialog open={open} onClose={()=>setOpen(false)} fullWidth maxWidth='xs'>
                <DialogTitle>Lock Article</DialogTitle>
                <DialogContent>
                    <DialogContentText> Set the minimum required Edit Level to edit this article </DialogContentText><br/>
                    <TextField value={newEditLevel} onChange={(e)=>setNewEditLevel(e.target.value)} fullWidth type='number'/>
                </DialogContent>
                <DialogActions>
                    <Button color='secondary' disabled={inProgress} onClick={onOK}>OK</Button>
                    <Button color='secondary' disabled={inProgress} onClick={()=>{setOpen(false); setNewEditLevel(props.level)}}>Cancel</Button>
                </DialogActions>
            </Dialog>

            <AlertSnackbar open={SnackOpen} setOpen={setSnackOpen} result={result} />

        </>
    )
}