import { Delete } from '@mui/icons-material';
import { Button, Dialog, DialogActions, DialogContent, DialogContentText, DialogTitle, IconButton, Tooltip } from '@mui/material';
import React, { useState } from 'react';
import { useHistory } from "react-router-dom";
import { DeleteImage } from '../../API/Image';
import AlertSnackbar from '../Reusable/AlertSnackbar';

export function DeleteImageButton(props) {

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

    const onOK = () => { DeleteImage(setInProgress, props.Session, props.id, onUpdate, onError) }

    return (<>

        <Tooltip title='Delete this Image'><IconButton onClick={()=>setOpen(true)}><Delete /></IconButton></Tooltip>

        <Dialog open={open} onClose={() => setOpen(false)} fullWidth maxWidth='xs'>
            <DialogTitle>Delete this image?</DialogTitle>
            <DialogContent>
                <DialogContentText>Are you sure you want to do this? Some articles may use this image and it may cause problemas</DialogContentText><br />
            </DialogContent>
            <DialogActions>
                <Button color='secondary' disabled={inProgress} onClick={onOK}>OK</Button>
                <Button color='secondary' disabled={inProgress} onClick={() => { setOpen(false) }}>Cancel</Button>
            </DialogActions>
        </Dialog>

        <AlertSnackbar open={SnackOpen} setOpen={setSnackOpen} result={result} />


    </>)
}