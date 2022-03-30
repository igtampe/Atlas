import React, {useState} from 'react';
import {Card, CardContent, Divider, CircularProgress, Link, IconButton, Tooltip } from '@mui/material';
import { APIImage, GetImageInfo } from '../API/Image';
import { ContentCopy } from '@mui/icons-material';
import { DeleteImageButton } from './ImageComponents/DeleteImageButton';

export default function ImageViewer(props) {

    const [image, setImage] = useState(undefined)
    const [error, setError] = useState(undefined)
    const [loading, setLoading] = useState(false)

    if(!image && !loading){
        GetImageInfo(setLoading,props.id,setImage,setError)
    } 

    if (!image) {
        return (<>
            <div style={{ textAlign: 'center', marginTop: '50px', marginBottom: '50px' }}>
                <CircularProgress /><br /><br />Loading Image Information
            </div>
        </>)
    }

    if(error){
        return(<>
            <h1>oops</h1>
            <Divider/>
            An error occurred: {error}
        </>)
    }

    return (<>
        <Card style={{ margin: '20px' }}>
            <CardContent>
            <table width='100%'>
                <tr>
                    <td width={props.Vertical ? '100%' : '50%'}>
                        <div style={{margin:'30px'}}>
                            <APIImage image={image} width='100%'/>
                        </div>
                    </td>
                    {props.Vertical ? <></> : <td width='50%'><ImageInfoDisplay image={image} User={props.User} Session={props.Session}/></td>}
                </tr>
                {!props.Vertical ? <></> : <tr><td width='100%'><ImageInfoDisplay image={image}  User={props.User} Session={props.Session}/></td></tr>}
            </table>
            </CardContent>
        </Card></>
    );
}

function ImageInfoDisplay(props){

    let image = props.image

    const articleText = () => { return "[IMG | " + image.name + " | " + image.id + " | " + image.description + " ]" }
    const toClipboard = () => { navigator.clipboard.writeText(articleText()) }

    return(<>

    <table width='100%'>
        <tr>
            <td><div style={{ fontSize: 35 }}>{image.name}</div></td>
            { props.User && props.User.isAdmin 
                ? <td width={1}><DeleteImageButton id={image.id} Session={props.Session}/></td>
                : <></>
            }
        </tr>
    </table>
    <Divider/>
    <table width='100%'>
        <tr><td>Uploaded on: </td><td>{(new Date(image.dateUploaded)).toString()}</td></tr>
        <tr><td>Uploaded by: </td><td><UserMicroCard user={image.uploader}/></td></tr>
    </table>
    <p>{image.description}</p><br/>
    <p>To use this image in an article:</p>
    <table>
        <tr>
            <td style={{outline:'2px solid white'}}><code>{articleText()}</code></td>
            <td><Tooltip title="Copy to Clipboard"><IconButton style={{marginLeft:'10px'}} onClick={toClipboard}><ContentCopy/></IconButton></Tooltip></td>
        </tr>
    </table>
    
    </>)

}

function UserMicroCard(props){

    let user = props.user

    if(!user) {return (<></>)}

    return(<Link href={"/Article/" + user.username}>
        <table>
            <tr>
                <td><img alt='User PFP' src={user.imageURL === "" ? "/icons/person.png" : user.imageURL} height='20px'/></td>
                <td>{user.username}</td>
            </tr>
        </table>
    </Link>)

}
