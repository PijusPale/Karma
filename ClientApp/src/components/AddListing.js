import React from "react";
import { useForm } from "react-hook-form";
import axios from "axios";
import { Redirect } from 'react-router';

export default function AddListing() {
    const { register, handleSubmit, formState } = useForm({ mode: 'onBlur', defaultValues: { Quantity: 1, Location: "Lithuania" }, shouldUseNativeValidation: true });
    const { isSubmitting, isSubmitted } = formState;

    const onSubmit = data => {
        var imageAttached = false;
        const formData = new FormData();
        if (data.ImagePath.length > 0) {
            formData.append('FormFile', data.ImagePath[0]);
            formData.append('FileName', data.ImagePath[0].name);
            data.ImagePath = 'images/' + data.ImagePath[0].name;
            imageAttached = true;
        }
        else {
            data.ImagePath = 'images/default.png';
        }
        fetch('listing', {
            mode: 'cors',
            method: 'POST',
            headers: { 'Content-type': 'application/json' },
            body: JSON.stringify(data),
        }).then(function () {
            if (imageAttached) {
                const res = axios.post('image', formData);
                console.log(res);
            }
        }).then(function () {
        });
    };

    return (
        <form onSubmit={handleSubmit(onSubmit)}>
            <div><p>Title:</p>
                <input {...register("Name", {
                    required: "Title is required.",
                    maxLength: { value: 20, message: "Maximum length of 20 characters exceeded." },
                    pattern: { value: /^[a-zA-Z0-9)]+$/, message: "Please enter only A-Z letters, 0-9 numbers or ! sign." }
                })} />
                <p>Category:</p>
                <select {...register("Category", { required: true })}>
                    <option value="Vehicles">Vehicles</option>
                    <option value="Apparel">Apparel</option>
                    <option value="Electronics">Electronics</option>
                    <option value="Entertainment">Entertainment</option>
                </select>
                <p>Description:</p>
                <textarea {...register("Description", {
                    maxLength: { value: 200, message: "Maximum length of 500 characters exceeded." },
                    pattern: { value: /^[a-zA-Z0-9!+, ]+$/, message: "Please enter only A-Z letters, 0-9 numbers or !+ signs." }
                })} />
                <p>Quantity:</p>
                <input type="number" {...register("Quantity", { required: "Quantity of minimum 1 is required.", min: 1, max: 100 })} />
                <p>Location:</p>
                <input {...register("Location", { required: true })} />
                <p>Images:</p>
                <input asp-for="FileUpload.FormFile" type="file" name="temp-image" accept="image/*"{...register("ImagePath")} />
            </div>
            <button disabled={isSubmitting} className="btn btn-primary mr-1">
                {isSubmitting && <span className="spinner-border spinner-border-sm mr-1"></span>}
                Submit
            </button>
            {isSubmitted && <Redirect push to='/' />}
        </form>
    );
}