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
import { UserContext } from '../../UserContext';

export function LoginForm() {
  const { setLoggedIn, setUser: setCurrentUser } = useContext(UserContext);
  const { switchToSignup } = useContext(LoginContext);
 
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [incorrectUsernameOrPassword, setincorrectUsernameOrPassword] = useState(false);

  const toggle = () => {
      setincorrectUsernameOrPassword(false);
  };

  const onLogIn = async () => {
    const response = await fetch('user/authenticate', {
        method: 'POST',
        headers: { 'Content-type': 'application/json' },
        body: JSON.stringify({ username, password }),
    });

    if (response.ok) {
        setincorrectUsernameOrPassword(false);
        const user = JSON.parse(await response.text());
        setCurrentUser(user);
        setLoggedIn(true);
        localStorage.setItem('token', user.token);
        toggle();
    }
    else {
        setincorrectUsernameOrPassword(true);
    }
};

  return (
    <BoxContainer>
      <FormContainer>
        <Input type="email" placeholder="Email/Username" onChange={e => setUsername(e.target.value)} />
        <Input type="password" placeholder="Password" onChange={e => setPassword(e.target.value)} />
      </FormContainer>
      <Marginer direction="vertical" margin={10} />
      <SmallText hidden={!incorrectUsernameOrPassword}>Username and Password don't match</SmallText>
      <Marginer direction="vertical" margin="1.6em" />
      <SubmitButton type="submit" onClick={onLogIn}>Log in</SubmitButton>
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