import { Backdrop, Button, CircularProgress, Dialog, DialogActions, DialogContent, DialogContentText, DialogTitle, Divider } from '@mui/material';
import React, { useState } from 'react';
import { useHistory } from "react-router-dom";
import { GetArticle, UpdateArticle } from '../API/Article';
import { MainPane } from './EditorComponents/MainPane';
import AlertSnackbar from './Reusable/AlertSnackbar';

export function ArticleEditor(props) {
    
    const history = useHistory()

    const [text, setText] = useState("")
    const [preview, setPreview] = useState(undefined)

    const [noEdit, setNoEdit] = useState(undefined)

    const [loading,setLoading] = useState(false)
    const [articleLoaded, setArticleLoaded] = useState(false)

    const [result, setResult] = useState({ severity: "success", text: "idk" })
    const [SnackOpen, setSnackOpen] = useState(false);

    const onLoad = (article) => {
        setArticleLoaded(true)
        setText(article.text)

        let editLevel = props.User ? props.User.editLevel : 0
        let isAdmin = props.User ? props.User.isAdmin : false
        setNoEdit(editLevel < article.editLevel && !isAdmin)
    
    }

    const onLoadError = () => {
        //Uh... Panic
        setNoEdit(true) //yeah this'll work
    }

    if(!loading && !articleLoaded){

        GetArticle(setLoading,props.title,onLoad,onLoadError)

    }
    

    const onPublishSuccess = () => {
        history.push("/Article/" + props.title)
    }

    const onPublishError = (e) => {
        setResult({ severity: "error", text: e });
        setSnackOpen(true)
        setLoading(false)
    }

    const onOk = () => {
        UpdateArticle(setLoading,props.Session,props.title,text,onPublishSuccess,onPublishError)
    }

    return(<>
        <div style={{ fontSize: 35 }}>Editing new article {props.title}</div>
        <Divider/><br/>
        <MainPane Session={props.Session} title={props.title} text={text} setText={setText} preview={preview} setPreview={setPreview}/>
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

        <Dialog open={noEdit}>
            <DialogTitle> Cannot edit this article</DialogTitle>
            <DialogContent>
                <DialogContentText>
                    This article requires an editor level which you do not have. Consider talking to an administrator to raise your editor level.
                </DialogContentText>
            </DialogContent>
            <DialogActions>
                <Button color="secondary" onClick={()=>{history.goBack()}}>Go Back</Button>
            </DialogActions>
        </Dialog>

        <AlertSnackbar open={SnackOpen} setOpen={setSnackOpen} result={result} />
    </>)
}