import * as React from 'react';
import {TextField, Button, IconButton} from "@mui/material";
import { useHistory } from "react-router-dom";
import { Search } from '@mui/icons-material';
import useQuery from '../Hooks/useQuery';

export default function SearchField(props) {

    let Uquery = useQuery();

    const history = useHistory();
    const [query,setQuery] = React.useState(Uquery.get("query"));

    const launchSearch = () => { 
        history.push("/Search?query=" + query) 
        history.go()
    }

    return (<table style={{width:'100%'}}>
        <tr>
            <td> <TextField placeholder="Search Articles" value={query} 
                onKeyDown={(e)=>{if(e.key==='Enter'){launchSearch()}}}
                onChange={(event)=>{setQuery(event.target.value)}} fullWidth/> </td>
            <td width={1}> 
                {props.icon
                    ? <IconButton onClick={launchSearch} style={{marginLeft:'20px'}}><Search/></IconButton>
                    : <Button variant='contained' onClick={launchSearch} style={{marginLeft:'20px'}}>Search</Button>
                }
            </td>
        </tr>
    </table>);
}