import { Settings } from "@mui/icons-material";
import { IconButton, Menu, MenuItem } from "@mui/material";
import React, { useState } from "react";

export default function SettingsButton(props) {

    const [anchorEl, setAnchorEl] = useState(null);
    const open = Boolean(anchorEl);
  
    const handleClick = (event) => { setAnchorEl(event.currentTarget); };
    const handleClose = () => { setAnchorEl(null); };
  
    return (
      <div>
        <IconButton onClick={handleClick} style={{ textTransform: 'none' }}> <Settings/> </IconButton>
        
        <Menu anchorEl={anchorEl} open={open} onClose={handleClose} >
          <MenuItem onClick={()=>{}}>My Profile</MenuItem>
          <MenuItem onClick={()=>{}}>Account Settings</MenuItem>
        </Menu>
      </div>
    );
  
  }
  