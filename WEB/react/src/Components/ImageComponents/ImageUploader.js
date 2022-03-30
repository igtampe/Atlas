import React, { useState } from "react";
import {
    Dialog, DialogActions, DialogContent, DialogTitle,
    CircularProgress, Button, TextField, Box, Divider
} from "@mui/material";
import AlertSnackbar from "../Reusable/AlertSnackbar";
import { UploadImage } from "../../API/Image";

export default function ImageUploader(props) {

    const [file, setFile] = useState(undefined)
 
    const [loading, setLoading] = useState(false);

    const [name, setName] = useState('');
    const [description, setDescription] = useState('');

    const [result, setResult] = useState({ severity: "success", text: "idk" })
    const [SnackOpen, setSnackOpen] = useState(false);

    const onSuccess = (data) => {
        setResult({ severity: 'success', text: 'Image uploaded!' })
        setSnackOpen(true)

        handleClose();
    }

    const onError = (error) => {
        setResult({ severity: 'danger', text: error })
        setSnackOpen(true)

    }

    const handleUpload = (event) => {
        UploadImage(setLoading, props.Session, file, name, description, onSuccess, onError)
    }

    const clearForm = (event) => { setFile(undefined) }

    const handleClose = (event) => {
        if (loading) { return; }
        props.setOpen(false)
        clearForm();
    }

    const updateFile = (event) => {
        if (!event.target.files || event.target.files.length < 0 || !event.target.files[0]) {
            console.warn('No files selected!')
            return
        }
        setFile(event.target.files[0])
    }

    const getImage = () => {

        if (!!file) { return (URL.createObjectURL(file)) }
        else { return ('icons/images.png') }

    }

    return (
        <React.Fragment>
            <Dialog fullWidth maxWidth="xs" open={props.open} onClose={handleClose}>
                <DialogTitle>Upload an Image</DialogTitle>
                <DialogContent>
                    <div style={{ textAlign: 'center', marginBottom: '20px' }}>
                        <img src={getImage()} alt="Picked" height="150px" />
                    </div>
                    {loading ? <div style={{ textAlign: 'center' }}> <CircularProgress /> </div> : <>
                        <table style={{ width: '100%' }}>
                            <tr><td>
                                <Box sx={{ display: 'flex', alignItems: 'flex-end' }}>
                                    <TextField label='File' value={file ? file.name : ''} fullWidth inputProps={{ readOnly: true, }} />
                                    <input accept="image/png,image/jpeg,image/gif" style={{ display: 'none' }} id="raised-button-file" type="file" onChange={updateFile} />
                                    <label htmlFor="raised-button-file"> <Button style={{ marginBottom: '10px', marginLeft: '10px' }} component="span"> Browse </Button> </label>
                                </Box>
                            </td></tr>
                            <tr><td><Divider style={{marginTop:'20px'}}/></td></tr>
                            <tr><td>
                                <TextField value={name} onChange={(e)=>setName(e.target.value)} label={'Name'} fullWidth style={{marginTop:'20px'}}/>
                            </td></tr>
                            <tr><td>
                                <TextField value={description} onChange={(e)=>setDescription(e.target.value)} label={'Description'} 
                                fullWidth multiline minRows={5} variant='filled' style={{marginTop:'20px'}}/>
                            </td></tr>
                        </table></>}
                </DialogContent>
                <DialogActions>
                    {loading ? <></> : <>
                        <Button onClick={handleUpload}>Upload</Button>
                        <Button onClick={handleClose}>Cancel</Button>
                    </>}
                </DialogActions>
            </Dialog>

            <AlertSnackbar open={SnackOpen} setOpen={setSnackOpen} result={result} />

        </React.Fragment>
    );

}
