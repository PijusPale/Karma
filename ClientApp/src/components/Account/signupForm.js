import React, { useContext, useState, Component } from "react";
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
  const [usernameDuplicateFound, setUsernameDuplicateFound] = useState(false);
  const [emailDuplicateFound, setEmailDuplicateFound] = useState(false);

  const onSignUp = async(data) => {
    const response = await fetch('user/signup', {
        method: 'POST',
        headers: { 'Content-type': 'application/json' },
        body: JSON.stringify(data),
    });
    if (response.ok){
      window.alert("Registration successful!");
      switchToSignin();
    }
    else if (response.status === 403){
      setUsernameDuplicateFound(true);
    }
    else if (response.status === 402){
      setEmailDuplicateFound(true);
    }
  };

  return (
    <BoxContainer >
      <FormContainer onSubmit={handleSubmit(onSignUp)}>
      <Input type="text" placeholder="Username" {...register("Username", {
                    required: "Username is required.",
                    pattern: { value: /^[a-zA-Z0-9]+$/, message: "Please enter only A-Z letters and 0-9 numbers" }
                })}/>
        <SmallText hidden={!usernameDuplicateFound}>This username already exists</SmallText>
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
        <SmallText hidden={!emailDuplicateFound}>This username already exists</SmallText>
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