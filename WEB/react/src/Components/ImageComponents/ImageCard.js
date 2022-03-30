import React, { useState } from 'react';
import { Button, Card, CardContent, Dialog, DialogContent, Tooltip } from '@mui/material';
import { APIImage } from '../../API/Image';
import ImageViewer from '../ImageViewer';

export default function ImageCard(props) {

    const [open, setOpen] = useState(false)

    const TooltipText=()=>{
        return(<>
            <b>{props.image.name}</b><br/>
            <i>{(new Date(props.image.dateUploaded)).toDateString()}</i><br/>
            <br/>
            {props.image.description}
            </>
        )
    }

    return (<div className='square' style={{ width: '200px', height:'200px', float: 'left', margin: '10px' }}>
        <Card style={{ margin: '20px', width: '200px' }}>
            <CardContent style={{textAlign:'center'}}>
                <Tooltip title={TooltipText()}>
                    <Button onClick={()=>setOpen(true)}>
                        <APIImage image={props.image} width='150px' />
                    </Button>
                </Tooltip>                
            </CardContent>
        </Card>

        <Dialog open={open} onClose={()=>setOpen(false)} fullWidth maxWidth='lg'>
            <DialogContent>
                <ImageViewer id={props.image.id} User={props.User} Session={props.Session} Vertical={props.Vertical}/>
            </DialogContent>
        </Dialog>

    </div >);
}