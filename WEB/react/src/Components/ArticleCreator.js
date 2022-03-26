import { Backdrop, Button, CircularProgress, Divider, TextField } from '@mui/material';
import React, { useState } from 'react';
import { useHistory } from "react-router-dom";
import { CreateArticle } from '../API/Article';
import { MainPane } from './EditorComponents/MainPane';
import AlertSnackbar from './Reusable/AlertSnackbar';
import useQuery from './Hooks/useQuery';

export function ArticleCreator(props) {
    
    const history = useHistory()
    let query = useQuery();

    const [text, setText] = useState("")
    const [preview, setPreview] = useState(undefined)
    const [title, setTitle] = useState(query.get("title"))

    const [loading,setLoading] = useState(false)

    const [result, setResult] = useState({ severity: "success", text: "idk" })
    const [SnackOpen, setSnackOpen] = useState(false);

    const onSuccess = () => {
        history.push("/Article/" + title)
    }

    const onError = (e) => {
        setResult({ severity: "error", text: e });
        setSnackOpen(true)
        setLoading(false)
    }

    const onOk = () => {

        CreateArticle(setLoading,props.Session,title,text,onSuccess,onError)
        
    }

    return(<>
        <div style={{ fontSize: 35 }}>Creating { !title || title==="" ? "new article" : title}</div>
        <Divider/><br/>
        <TextField value={title} onChange={(e)=>setTitle(e.target.value)} fullWidth label='Title' variant='outlined'/> <br/>
        <br/>
        <MainPane Session={props.Session} title={title} text={text} setText={setText} preview={preview} setPreview={setPreview}/>
        <br/>
        <Divider/>
        <table width='100%'>
            <tr>
                <td></td>
                <td width={1}><Button disabled={loading} color="secondary" variant='contained' style={{margin:'10px', width:'100px'}} onClick={onOk}>Save</Button></td>
                <td width={1}><Button disabled={loading} variant='contained' style={{margin:'10px', width:'100px'}} onClick={()=>{history.goBack()}} >Cancel</Button></td>
            </tr>
        </table>
        
        <Backdrop sx={{ color: '#fff', zIndex: (theme) => theme.zIndex.drawer + 1 }} open={loading}>
            <CircularProgress />
        </Backdrop>

        <AlertSnackbar open={SnackOpen} setOpen={setSnackOpen} result={result} />
    </>)
}