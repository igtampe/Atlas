import React, {useState} from 'react';
import { PreviewArticle } from '../../API/Article';
import { ArticleDisplay } from '../Article';

export function PreviewPane(props) {
    
    const [loading, setLoading] = useState(false);
    const [error,setError] = useState(false)

    if(!props.preview && props.open && props.setPreview && !loading && !error){
        console.log("Time to preview")
        PreviewArticle(setLoading,props.Session,props.title,props.text,props.setPreview,setError)
    }

    if(!props.open && (props.preview)){ props.setPreview(undefined) }
    if(!props.open && (error)){ props.setError(undefined) }

    return(<ArticleDisplay {...props} Article={props.preview} Error={error} noManage/>)
}