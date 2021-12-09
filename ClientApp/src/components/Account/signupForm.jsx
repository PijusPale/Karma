import React, { useContext } from "react";
import {
  BoldLink,
  BoxContainer,
  FormContainer,
  Input,
  MutedLink,
  SubmitButton,
  SmallText,
} from "./common";
import { Marginer } from "./marginer";
import { LoginContext } from "./LoginContext";

export function SignupForm(props) {
  const { switchToSignin } = useContext(LoginContext);

  return (
    <BoxContainer>
      <FormContainer>
        <Input type="text" placeholder="First Name" />
        <Input type="text" placeholder="Last Name" />
        <Input type="email" placeholder="Email" />
        <Input type="password" placeholder="Password" />
        <Input type="password" placeholder="Confirm Password" />
      </FormContainer>
      <Marginer direction="vertical" margin={10} />
      <SubmitButton type="submit">Sign up</SubmitButton>
      <Marginer direction="vertical" margin="1em" />
      <SmallText>
      Already have an account?
      </SmallText>
      <BoldLink href="#" onClick={switchToSignin}>
        Log in
      </BoldLink>
    </BoxContainer>
  );
}