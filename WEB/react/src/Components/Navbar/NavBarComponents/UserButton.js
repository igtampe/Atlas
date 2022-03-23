import React, { useState } from "react";
import { Button, Divider, ListItem, Menu, MenuItem } from "@mui/material";
import { APIURL, GenerateJSONPut } from "../../../API/common";
import PasswordChangeButton from "./PasswordChangeButton";
import PicturePicker from "../../Reusable/PicturePicker";
import AlertSnackbar from "../../Reusable/AlertSnackbar";
import { useHistory } from "react-router-dom";

export default function UserButton(props) {

    const history = useHistory();

    const [anchorEl, setAnchorEl] = React.useState(null);

    const [snackOpen, setSnackOpen] = useState(false)
    const [result, setResult] = useState({ text: 'a', severity: 'success' })

    const [pickerOpen, setPickerOpen] = useState(false)

    const open = Boolean(anchorEl);

    const handleClick = (event) => { setAnchorEl(event.currentTarget); };
    const handleClose = () => { setAnchorEl(null); };
    
    const handlePickerOpen = () => {
        setPickerOpen(true);
        handleClose();
    }

    const setImageURL = (e) => {

        console.log(e)

        if (!e) {
            console.log("imageurl was not set")
            return;
        }

        //ok we have a new image

        fetch(APIURL + '/API/Users/image', GenerateJSONPut(props.Session, e))
            .then(response => response.ok)
            .then(data => {
                console.log("ok it happeneds")
                if (!data) {
                    console.log("n o")
                    setResult({ text: "An error occured while updating your picture", severity: 'error' })
                    setSnackOpen(true);
                }
                else {
                    console.log("y e s")
                    props.RefreshUser();
                    setResult({ ...result, text: 'Picture updated successfully!' })
                    setSnackOpen(true);

                }
                setImageURL(undefined)
            }
            )
    }

    return (
        <div>
            <Button onClick={handleClick} style={{ textTransform: 'none' }}>
                <img src={props.User.imageURL === "" ? "/icons/person.png" : props.User.imageURL} alt="Profile" width="30px" style={{ margin: "5px", marginLeft: "10px" }} />
            </Button>
            <Menu anchorEl={anchorEl} open={open} onClose={handleClose} >
                <MenuItem onClick={()=>{history.push('/Article/' + props.User.username); handleClose()}} >My Profile</MenuItem>
                <MenuItem onClick={handlePickerOpen} disabled={!props.User.isAdmin && !props.User.isUploader}>Change Image</MenuItem>
                <Divider />
                <ListItem key="AccountManagement">
                    <PasswordChangeButton />
                </ListItem>
            </Menu>

            <AlertSnackbar open={snackOpen} setOpen={setSnackOpen} result={result} />

            <PicturePicker open={pickerOpen} setOpen={setPickerOpen} imageURL={props.User.imageUrl} setImageURL={setImageURL} defaultImage={"/icons/person.png"} />

        </div>
    );

}
