import React, { useState } from "react";
import {
  Dialog, DialogActions, DialogContent, DialogTitle,
  CircularProgress, Button, TextField, Box
} from "@mui/material";
import Cookies from 'universal-cookie';
import { APIURL } from "../../API/common";
import AlertSnackbar from "./AlertSnackbar";

const cookies = new Cookies();

export default function PicturePicker(props) {

  const [file, setFile] = useState(undefined)
  const [tempImageURL, setTempImageURL] = useState('')

  const [mode, setMode] = useState(0);

  const [loading, setLoading] = useState(false);
  
  const [result, setResult] = useState({ severity: "success", text: "idk" })
  const [SnackOpen, setSnackOpen] = useState(false);

  const handleUpload = (event) => {
    
    //This will probably make some react developer cry.
    //but it WORKS (Who knows ow slow it is though)

    const FR = new FileReader();
    FR.addEventListener('load',(event)=>{
      
      setLoading(true)
      
      const requestOptions = {
        method: 'POST',
        headers: { 'Content-Type': file.type, 'Content-Length':file.size, 'SessionID': cookies.get('SessionID') },
        body: event.target.result
      };
  
      fetch(APIURL + "/API/Images", requestOptions)
        .then(response => {
          if (!response.ok) { console.error(response); }
          return response.text()
        }).then(data => {
          setLoading(false)
          if(data.includes('-')){
            //Assume we got an ID
            props.setImageURL(APIURL + '/API/Images/' + data.substring(1).substring(0,data.length-2))
            handleClose();
            setResult({severity:'success', text:'Image uploaded!'})
            setSnackOpen(true)
          } else {
            //There's been a problem
            setResult({severity:'danger', text:data})
            setSnackOpen(true)
          }
        })
        .catch( e => {
          setLoading(false)
          setResult({severity:'danger', text:'An error occurred when sending the data over'})
          setSnackOpen(true)}
        )
    })

    FR.readAsArrayBuffer(file)
  }

  const handleOK = (event) => {

    if(mode===0){
      props.setImageURL(tempImageURL)
      handleClose();
    } else if (mode===1){ handleUpload();}

  }

  const clearForm = (event) => {

    setMode(0)
    setFile(undefined)
    setTempImageURL('')

  }

  const handleClose = (event) => {
    if(loading) {return;}
    props.setOpen(false)
    clearForm();
  }

  const updateTempURL = (event) => {
    if (mode !== 0) { 
      setMode(0) 
      setFile(undefined)
    }
    setTempImageURL(event.target.value)
  }

  const updateFile = (event) => {
    if(!event.target.files || event.target.files.length < 0 || !event.target.files[0]) {
      console.warn('No files selected!')
      return
    }

    if (mode !== 1) { 
      setMode(1) 
      setTempImageURL('')
    }
    setFile(event.target.files[0])
  }

  const getImage = () => {

    if (mode===0) { return(tempImageURL === '' ? props.defaultImage : tempImageURL) } 
    else if (mode===1) {  return (URL.createObjectURL(file) ) } 
    else { return(props.defaultImage)}

  }

  return (
    <React.Fragment>
      <Dialog fullWidth maxWidth="xs" open={props.open} onClose={handleClose}>
        <DialogTitle>Select a Photo</DialogTitle>
        <DialogContent>

          <div style={{ textAlign: 'center', marginBottom: '20px' }}>
            <img src={getImage()} alt="Picked" height="150px" />
          </div>
          {loading ? 
          <div style={{textAlign:'center'}}>
            <CircularProgress/>
            </div>
          : <>
          <table style={{ width: '100%' }}>
            <tr>
              <td colSpan={2}>
                <TextField label='Image URL ' value={tempImageURL} onChange={updateTempURL} fullWidth />
              </td>
            </tr>
            <tr> <td colSpan={2} style={{ textAlign: 'center' }}> <div style={{ marginTop: '20px' }}>or<br/><br/></div></td></tr>
            <tr>
              <td>
                <Box sx={{ display: 'flex', alignItems: 'flex-end' }}>
                  <TextField label='File' value={file ? file.name : ''} fullWidth inputProps={{ readOnly: true, }} />
                  <input accept="image/png,image/jpeg,image/gif" style={{ display: 'none' }} id="raised-button-file" type="file" onChange={updateFile} />
                  <label htmlFor="raised-button-file">
                    <Button component="span"> Browse </Button> </label>
                </Box>
              </td>
            </tr>
          </table></>}
        </DialogContent>
        <DialogActions>
          {loading? <></>: <>
            <Button onClick={handleOK}>{mode===1 ? 'Upload' : 'Set'}</Button>
            <Button onClick={handleClose}>Cancel</Button>
          </>}
        </DialogActions>
      </Dialog>

      <AlertSnackbar open={SnackOpen} setOpen={setSnackOpen} result={result}/>

    </React.Fragment>
  );

}
