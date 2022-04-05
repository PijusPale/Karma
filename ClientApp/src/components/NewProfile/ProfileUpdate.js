import React, { useContext, useState, Component } from "react";
import {
    BoxContainer,
    FormContainer,
    Input,
    SubmitButton,
    SmallText,
} from "../Account/common";
import { Marginer } from "../Account/marginer";
import { LoginContext } from "../Account/LoginContext";
import { useForm } from "react-hook-form";
import { UserContext } from "../../UserContext";

export function ProfileUpdate(props) {
    const { loggedIn, user: currentUser } = useContext(UserContext);
    const { switchToSignin } = useContext(LoginContext);
    const { register, handleSubmit } = useForm({});
    const [usernameDuplicateFound, setUsernameDuplicateFound] = useState(false);
    const [emailDuplicateFound, setEmailDuplicateFound] = useState(false);

    const onSave = async (data) => {
        const response = await fetch('user/update', {
            method: 'POST',
            headers: { 'Content-type': 'application/json' },
            body: JSON.stringify(data),
        });
        if (response.ok) {
            window.alert("Update successful!");
        }
        else if (response.status === 403) {
            setUsernameDuplicateFound(true);
        }
        else if (response.status === 402) {
            setEmailDuplicateFound(true);
        }
    };

    return (
        <BoxContainer >
            <FormContainer onSubmit={handleSubmit(onSave)}>
                <Input type="text" defaultValue={currentUser.username} {...register("Username", {
                    required: "Username is required.",
                    pattern: { value: /^[a-zA-Z0-9]+$/, message: "Please enter only A-Z letters and 0-9 numbers" }
                })} />
                <SmallText hidden={!usernameDuplicateFound}>This username already exists</SmallText>
                <Input type="text" defaultValue={currentUser.firstName} {...register("FirstName", {
                    required: "First Name is required.",
                    pattern: { value: /^[a-zA-Z]+$/, message: "Please enter only A-Z letters" }
                })} />
                <Input type="text" defaultValue={currentUser.lastName} {...register("LastName", {
                    required: "Last Name is required.",
                    pattern: { value: /^[a-zA-Z]+$/, message: "Please enter only A-Z letters" }
                })} />
                <Input type="email" defaultValue={currentUser.email} {...register("Email", {
                    required: "Email is required.",
                    pattern: { value: /^[a-zA-Z0-9@.]+$/, message: "Please enter only A-Z letters, 0-9 numbers or @ and . signs." }
                })} />
                <SmallText hidden={!emailDuplicateFound}>This email already exists</SmallText>
                <Input type="password" defaultValue={currentUser.password} {...register("Password", {
                    required: "Password is required.",
                    pattern: { value: /^[a-zA-Z0-9!]+$/, message: "Please enter only A-Z letters, 0-9 numbers or a ! sign." }
                })} />
                <Input contentEditable="false" type="number" value={currentUser.id} {...register("Id", {
                    required: "ID is required.",
                    pattern: { value: /^[0-9]+$/, message: "Please enter only numbers" },
                })} />

                <Marginer direction="vertical" margin={10} />
                <SubmitButton type="submit">Save</SubmitButton>
                <Marginer direction="vertical" margin="1em" />
            </FormContainer>
        </BoxContainer>
    );
}