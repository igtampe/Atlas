import { Delete } from '@mui/icons-material';
import { Button, Dialog, DialogActions, DialogContent, DialogContentText, DialogTitle, IconButton, Tooltip } from '@mui/material';
import React, { useState } from 'react';
import { useHistory } from "react-router-dom";
import { DeleteArticle } from '../../../API/Article';
import AlertSnackbar from '../../Reusable/AlertSnackbar';

export function DeleteBButton(props) {

    const history = useHistory();

    const [open, setOpen] = useState(false);
    const [inProgress, setInProgress] = useState(false);

    const [SnackOpen, setSnackOpen] = useState(false)
    const [result, setResult] = useState("")

    const onUpdate = () => { history.goBack(); }

    const onError = (reason) => {

        setResult({ severity: "error", text: reason })
        setSnackOpen(true)

    }

    const onOK = () => {
        DeleteArticle(setInProgress, props.Session, props.title, onUpdate, onError)
    }


    if (!props.User || !props.User.isAdmin) { return (<></>) } //Hide the button if you're not an admin
    return (<>

        <Tooltip title='Delete this Article'><IconButton onClick={()=>setOpen(true)}><Delete /></IconButton></Tooltip>

        <Dialog open={open} onClose={() => setOpen(false)} fullWidth maxWidth='xs'>
            <DialogTitle>Delete Article?</DialogTitle>
            <DialogContent>
                <DialogContentText>Are you sure you want to do this?</DialogContentText><br />
            </DialogContent>
            <DialogActions>
                <Button color='secondary' disabled={inProgress} onClick={onOK}>OK</Button>
                <Button color='secondary' disabled={inProgress} onClick={() => { setOpen(false) }}>Cancel</Button>
            </DialogActions>
        </Dialog>

        <AlertSnackbar open={SnackOpen} setOpen={setSnackOpen} result={result} />


    </>)
}