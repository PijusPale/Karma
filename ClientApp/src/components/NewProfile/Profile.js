import React, { useContext, useState } from 'react';
import { Button, Modal, NavLink } from 'reactstrap';
import { UserContext } from "../../UserContext";
import { UpdateProfileBox } from "../Account/UpdateProfileLayout";
import './profile.css'
import { ConfirmationButton } from '../ConfirmationButton';
import {GardenComp} from "../../garden/garden"

function formatted_date()
{
   var result="";
   var d = new Date();
   result += d.getFullYear()+"/"+(d.getMonth()+1)+"/"+d.getDate();
   return result;
}

function timeSince(date) { //UTC format
  const rtf = new Intl.RelativeTimeFormat("en", {
      localeMatcher: "best fit",
      numeric: "always",
      style: "long",
  });
  var seconds = Math.floor((new Date() - new Date(date + "Z")) / 1000);

  var interval = seconds / 31536000;

  if (interval > 1) {
      return rtf.format(-Math.floor(interval), 'year');
  }
  interval = seconds / 2592000;
  if (interval > 1) {
      return rtf.format(-Math.floor(interval), 'month');
  }
  interval = seconds / 86400;
  if (interval > 1) {
      return rtf.format(-Math.floor(interval), 'day');
  }
  interval = seconds / 3600;
  if (interval > 1) {
      return rtf.format(-Math.floor(interval), 'hour');
  }
  interval = seconds / 60;
  if (interval > 1) {
      return rtf.format(-Math.floor(interval), 'minute');
  }
  return 'now';
}
export const Profile = () => {
  const [modal, setModal] = useState(false);
  const toggle = () => {
    setModal(!modal);
  };
  const { loggedIn, user: currentUser } = useContext(UserContext);

  const clickDeleteProfile = async () => {
    alert("You want to delete your profile!");
    const response = await fetch('user/delete', {
      method: 'POST',
      headers: { 'Content-type': 'application/json' },
      body: JSON.stringify(currentUser),
    });
  }
  
  return (loggedIn ?

    <div>
      <div class="sidenav">
        <div class="profile">
          <img src={currentUser.avatarPath} width="100" height="100" />

          <div class="name">
            {loggedIn && currentUser.username}
          </div>
          <div class="job">
            Last active: {timeSince(currentUser.lastActive)}
          </div>
          <div>
            <div class="divider" />
          </div>
          <div>
            <div>
              <Button outline color="success" onClick={toggle}>Change profile details</Button>
              <Modal className='Modal' autoFocus={false} isOpen={modal} toggle={toggle} size="sm">
                <UpdateProfileBox />
              </Modal>
            </div>
          </div>
          <div>
            <div class="divider" />
          </div>
          <div>
            <ConfirmationButton color="danger" onSubmit={clickDeleteProfile} submitLabel={'Delete'}
              prompt={'Are you sure you want to delete your profile?'}>Delete profile</ConfirmationButton>
          </div>
        </div>

        {/* <div class="sidenav-url">
      <div class="url">
        <a href="#profile" class="active">Profile</a>
        <hr align="center"/>
      </div>
      <div class="url">
        <a href="#settings">Settings</a>
        <hr align="center"/>
      </div>
    </div> */}
      </div>
      <div class="main">
        <h2>IDENTITY</h2>
        <div class="card">
          <div class="card-body">
            <i class="fa fa-pen fa-xs edit"></i>
            <table>
              <tbody>
                <tr>
                  <td>Name</td>
                  <td>:</td>
                  <td>{loggedIn && currentUser.firstName}</td>
                </tr>
                <tr>
                  <td>Lastname</td>
                  <td>:</td>
                  <td>{loggedIn && currentUser.lastName}</td>
                </tr>
                <tr>
                  <td>Email</td>
                  <td>:</td>
                  <td>{loggedIn && currentUser.email}</td>
                </tr>
                <tr>
                  <td>Signup date</td>
                  <td>:</td>
                  <td>2022-01-{currentUser.id}</td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>
      </div>
      <div>
        <GardenComp username={currentUser.username}></GardenComp>
      </div>
    </div>
    :
    <div>
      You must log in to review your profile!
    </div>

  )

}