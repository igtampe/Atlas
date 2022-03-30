import React, { useState } from 'react';
import { CircularProgress, Divider, TextField, IconButton } from '@mui/material';
import { GetImages } from '../API/Image';
import { Search, Upload, NavigateNext, NavigateBefore } from '@mui/icons-material';
import ImageUploader from './ImageComponents/ImageUploader';
import ImageCard from './ImageComponents/ImageCard'

export default function ImageBrowser(props) {

    const [images, setImages] = useState(undefined)
    const [loading, setLoading] = useState(undefined)

    const [query, setQuery] = useState("")
    const [displayed, setDisplayed] = useState(0)
    const [noMas, setNoMas] = useState(false)

    const [uploaderOpen, setUploaderOpen] = useState(false)

    const onSuccess = (back) => (data) => {

        setImages(data)
        if(back){
            console.log("Going Back!")
            setDisplayed(displayed - 20)
            setNoMas(false)
        } else {
            console.log("Going Forward!")
            setDisplayed(displayed + 20)
            setNoMas(displayed + data.length === 0 || displayed + data.length % 20 !== 0)
        }

    }

    const getMas = () => GetImages(setLoading, onSuccess(false), query, displayed     , 20)
    const goBack = () => GetImages(setLoading, onSuccess(true),  query, displayed - 40, 20) 

    const onUploaderChange = (val) => {
        setUploaderOpen(val);
        if (!val) { onSearch() }
    }

    const onSearch = () => {
        setDisplayed(0)
        setImages(undefined)
    }

    if (!images && !loading) { getMas() }

    if (!images) {
        return (<>
            <div style={{ textAlign: 'center', marginTop: '50px', marginBottom: '50px' }}>
                <CircularProgress /><br /><br />Loading Images
            </div>
        </>)
    }

    return (<>

        <table width='100%'>
            <tr>
                <td><TextField placeholder='Search Images' value={query} onChange={(e) => setQuery(e.target.value)} fullWidth /></td>
                <td width={1}><IconButton onClick={onSearch} style={{ marginLeft: '10px' }}><Search /></IconButton></td>
                {
                    props.User && (props.User.isUploader || props.User.isAdmin)
                        ? <td width={1}><IconButton onClick={() => setUploaderOpen(true)} style={{ marginLeft: '10px' }}><Upload /></IconButton></td>
                        : <></>
                }
            </tr>
        </table>
        <Divider style={{ marginTop: '20px', marginBottom: '20px' }} />

        {!images
            ? <div style={{ textAlign: 'center', marginTop: '50px', marginBottom: '50px' }}><CircularProgress /><br /><br />Loading Images</div>
            : <> {images.length === 0
                ? <></>
                : <div> {images.map(a => <ImageCard image={a} User={props.User} Session={props.Session} Vertical={props.Vertical} />)} </div>} </>}

        <div style={{ marginTop: '20px', textAlign: 'center', clear: 'both' }}>
            {!loading ? <>
                <IconButton disabled={displayed<=20} variant='contained' color='secondary' onClick={goBack} style={{marginRight:'20px'}}><NavigateBefore/></IconButton>
                <IconButton disabled={noMas} variant='contained' color='secondary' onClick={getMas} style={{marginLeft:'20px'}}> <NavigateNext/></IconButton>
            </> : <></>}
        </div>

        <ImageUploader {...props} open={uploaderOpen} setOpen={onUploaderChange} />

    </>);
}