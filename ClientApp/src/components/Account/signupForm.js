import React, { useContext, useState } from "react";
import {
  BoldLink,
  BoxContainer,
  FormContainer,
  Input,
  SubmitButton,
  SmallText,
} from "./common";
import { Marginer } from "./marginer";
import { LoginContext } from "./LoginContext";
import { useForm } from "react-hook-form";

export function SignupForm(props) {
  
  const { switchToSignin } = useContext(LoginContext);
  const { register, handleSubmit } = useForm({});
  const { DuplicateFound, setDuplicateFound } = useState([]);

  const onSignUp = async(data) => {
    const response = fetch('user/signup', {
        method: 'POST',
        headers: { 'Content-type': 'application/json' },
        body: JSON.stringify(data),
    });
    if (response.status === 403){
      setDuplicateFound(true);
    }


};
/*  const onSignUp = data => {
    const response = fetch('user/Duplicate', {
      method: 'GET',
      headers: { 'Content-type': 'application/json' },
    })

    if (response.ok){
      setDuplicateFound(false); 
      fetch('user/signup', {
        method: 'POST',
        headers: { 'Content-type': 'application/json' },
        body: JSON.stringify(data),
    });
    }

    if (response.status === 500) {
      setDuplicateFound(true);
    }
};*/

  return (
    <BoxContainer onSubmit={handleSubmit(onSignUp)} >
      <FormContainer>
      <Input type="text" placeholder="Username" {...register("Username", {
                    required: "Username is required.",
                    pattern: { value: /^[a-zA-Z0-9]+$/, message: "Please enter only A-Z letters and 0-9 numbers" }
                })}/>
        <SmallText hidden={!DuplicateFound}>This username already exists</SmallText>
        <Input type="text" placeholder="First Name" {...register("FirstName", {
                    required: "First Name is required.",
                    pattern: { value: /^[a-zA-Z]+$/, message: "Please enter only A-Z letters" }
                })}/>
        <Input type="text" placeholder="Last Name" {...register("LastName", {
                    required: "Last Name is required.",
                    pattern: { value: /^[a-zA-Z]+$/, message: "Please enter only A-Z letters" }
                })}/>
        <Input type="email" placeholder="Email" {...register("Email", {
                    required: "Email is required.",
                    pattern: { value: /^[a-zA-Z0-9@.]+$/, message: "Please enter only A-Z letters, 0-9 numbers or @ and . signs." }
                })}/>
        <Input type="password" placeholder="Password" {...register("Password", {
                    required: "Password is required.",
                    pattern: { value: /^[a-zA-Z0-9!]+$/, message: "Please enter only A-Z letters, 0-9 numbers or a ! sign." }
                })}/>

      <Marginer direction="vertical" margin={10} />
      <SubmitButton type="submit">Sign up</SubmitButton>
      <Marginer direction="vertical" margin="1em" />
      </FormContainer>
      <SmallText>
      Already have an account?
      </SmallText>
      <BoldLink href="#" onClick={switchToSignin}>
        Log in
      </BoldLink>
    </BoxContainer>
  );
}