import React from "react";
import { useForm } from "react-hook-form";
import axios from "axios";
import { Redirect } from 'react-router';

export default function AddListing(props) {
    const { register, handleSubmit, formState } = useForm({ mode: 'onBlur', defaultValues: { Quantity: 1, Location: "Lithuania" }, shouldUseNativeValidation: true });
    const { isSubmitting, isSubmitted } = formState;
    const maxTitleLength = 20, maxDescriptionLength = 200;

    const onSubmit = data => {
        var imageAttached = false;
        data.id = props.id;
        const formData = new FormData();
        if (data.ImagePath.length > 0) {
            formData.append('FormFile', data.ImagePath[0]);
            formData.append('Name', data.ImagePath[0].name);
            data.ImagePath = 'images/' + data.ImagePath[0].name;
            imageAttached = true;
        }
        else {
            data.ImagePath = props.imagePath || 'images/default.png';
        }
        fetch('listing' + (props.update ? `/${props.id}` : ''), {
            mode: 'cors',
            method: props.update ? 'PUT' : 'POST',
            headers: {
                'Content-type': 'application/json',
                'Authorization': `Bearer ${localStorage.getItem('token')}`
            },
            body: JSON.stringify(data),
        }).then(function (response) {
            console.log(response.ok);
            if (imageAttached && response.ok) {
                const res = axios.post('image', formData);
                console.log(res);
            }
        });
        props.update && props.afterSubmit();
    };

    return (
        <form onSubmit={handleSubmit(onSubmit)}>
            <div class="form-group"><label>Title</label>
                <input defaultValue={props.name} class="form-control form-control-lg" {...register("Name", {
                    required: "Title is required.",
                    maxLength: { value: maxTitleLength, message: "Maximum length of " + { maxTitleLength } + " characters exceeded." },
                    pattern: { value: /^[a-zA-Z0-9! ]+$/, message: "Please enter only A-Z letters, 0-9 numbers or ! sign." }
                })} />      
                <label>Category</label>
                <select defaultValue={props.category} class="custom-select" {...register("Category", { required: true })}>
                    <option value="Vehicles">Vehicles</option>
                    <option value="Apparel">Apparel</option>
                    <option value="Electronics">Electronics</option>
                    <option value="Entertainment">Entertainment</option>
                </select>
                <label>Description</label>
                <textarea defaultValue={props.description} class="form-control" {...register("Description", {
                    maxLength: { value: maxDescriptionLength, message: "Maximum length of " + {maxDescriptionLength} + " characters exceeded." },
                    pattern: { value: /^[a-zA-Z0-9!+, ]+$/, message: "Please enter only A-Z letters, 0-9 numbers or !+ signs." }
                })} />
                <label>Quantity</label>
                <input type="number" class="form-control" {...register("Quantity", { required: "Quantity of minimum 1 is required.", min: 1, max: 100 })} />
                <label>Location</label>
                <input defaultValue={props.location} class="form-control"{...register("Location", { required: true })} />
                <label>Images</label>
                <input asp-for="FileUpload.FormFile" type="file" class="form-control-file" name="temp-image" accept="image/*"{...register("ImagePath")} />
            </div>
            <button disabled={isSubmitting} className="btn btn-primary mr-1">
                {isSubmitting && <span className="spinner-border spinner-border-sm mr-1"></span>}
                Submit
            </button>
            {isSubmitted && <Redirect push to='/' />}
        </form>
    );
}