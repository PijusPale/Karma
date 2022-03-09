import React from "react"
import './profile.css'
export const Profile = () =>{
    return(

    <div>
    <div class="sidenav">
    <div class="profile">
      <img src="https://as2.ftcdn.net/v2/jpg/03/32/59/65/1000_F_332596535_lAdLhf6KzbW6PWXBWeIFTovTii1drkbT.jpg" alt="" width="100" height="100"/>

      <div class="name">
        Jokubas
      </div>
      <div class="job">
        Zemaitija
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
                  <td>Jokubas</td>
                </tr>
                <tr>
                  <td>Email</td>
                  <td>:</td>
                  <td>jokubas@gmail.com</td>
                </tr>
                <tr>
                  <td>Address</td>
                  <td>:</td>
                  <td>Plunge, Lithuania</td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>
      </div>
      </div> 
    )

}