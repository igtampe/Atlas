import * as React from 'react';
import {TextField, Button, IconButton} from "@mui/material";
import { useHistory } from "react-router-dom";
import { Search } from '@mui/icons-material';

export default function SearchField(props) {

    const history = useHistory();
    const [query,setQuery] = React.useState("");

    const launchSearch = () => { 
        history.push("/Search?query=" + query) 
        history.go()
    }

    return (<table style={{width:'100%'}}>
        <tr>
            <td> <TextField placeholder="Search" value={query} onChange={(event)=>{setQuery(event.target.value)}} fullWidth/> </td>
            <td width={1}> 
                {props.icon
                    ? <IconButton onClick={launchSearch} style={{marginLeft:'20px'}}><Search/></IconButton>
                    : <Button variant='contained' onClick={launchSearch} style={{marginLeft:'20px'}}>Search</Button>
                }
            </td>
        </tr>
    </table>);
}