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
import { UserContext } from "../../UserContext";

export function LoginForm(props) {
  const { switchToSignup } = useContext(UserContext);

  return (
    <BoxContainer>
      <FormContainer>
        <Input type="email" placeholder="Email/Username" />
        <Input type="password" placeholder="Password" />
      </FormContainer>
      <Marginer direction="vertical" margin={10} />
      <BoldLink href="#">Forgot your password?</BoldLink>
      <Marginer direction="vertical" margin="1.6em" />
      <SubmitButton type="submit">Log in</SubmitButton>
      <Marginer direction="vertical" margin="1em" />
      <SmallText>
        Don't have an account?{" "}
        </SmallText>
        <BoldLink href="#" onClick={switchToSignup}>
          Sign up
        </BoldLink>
    </BoxContainer>
  );
}