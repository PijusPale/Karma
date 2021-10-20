import React from "react";
import { useForm } from "react-hook-form";
import axios from "axios";
import { Redirect } from 'react-router';

export default function AddListing(props) {
    const { register, handleSubmit, formState } = useForm({
        mode: 'onBlur',
        defaultValues: {
            Quantity: props.quantity || 1,
            "Location.Country": (props.location && props.location.country) || "Lithuania",
            "Location.District": (props.location && props.location.district) || "Zemaitija",
            "Location.City": (props.location && props.location.city) || "Šiauliai",
            "Location.RadiusKM": (props.location && props.location.radiusKM) || 5,
        }, shouldUseNativeValidation: true
    });
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
            <div className="form-group">
                <label>Title</label>
                <input defaultValue={props.name} className="form-control form-control-lg" {...register("Name", {
                    required: "Title is required.",
                    maxLength: { value: maxTitleLength, message: "Maximum length of " + { maxTitleLength } + " characters exceeded." },
                    pattern: { value: /^[a-zA-Z0-9! ]+$/, message: "Please enter only A-Z letters, 0-9 numbers or ! sign." }
                })} />
            </div>
            <div className="form-group">
                <label>Category</label>
                <select defaultValue={props.category} className="custom-select" {...register("Category", { required: true })}>
                    <option value="Vehicles">Vehicles</option>
                    <option value="Apparel">Apparel</option>
                    <option value="Electronics">Electronics</option>
                    <option value="Entertainment">Entertainment</option>
                </select>
            </div>
            <div className="form-group">
                <label>Description</label>
                <textarea defaultValue={props.description} className="form-control" {...register("Description", {
                    maxLength: { value: maxDescriptionLength, message: "Maximum length of " + { maxDescriptionLength } + " characters exceeded." },
                    pattern: { value: /^[a-zA-Z0-9!+, ]+$/, message: "Please enter only A-Z letters, 0-9 numbers or !+ signs." }
                })} />
            </div>
            <div className="form-group">
                <label>Quantity</label>
                <input type="number" className="form-control" {...register("Quantity", { required: "Quantity of minimum 1 is required.", min: 1, max: 100 })} />
            </div>
            <div className="form-group">
                <label>Country</label>
                <input className="form-control" {...register("Location.Country", { required: true, message: "Country is required." })} />
                <label>District</label>
                <input className="form-control" {...register("Location.District")} />
                <label>City</label>
                <input className="form-control" {...register("Location.City", { required: true, message: "City is required." })} />
                <label>Radius in kilometers</label>
                <input type="number" className="form-control" {...register("Location.RadiusKM", { required: true, message: "Radius of minimum 1 km is required.", min: 1, max: 100 })} />
                <small class="form-text text-muted"> Radius describes how close you are located from the city center.</small>
            </div>
            <div className="form-group">
                <label>Images</label>
                <input asp-for="FileUpload.FormFile" type="file" className="form-control-file" name="temp-image" accept="image/*"{...register("ImagePath")} />
            </div>
            <button disabled={isSubmitting} className="btn btn-primary mr-1">
                {isSubmitting && <span className="spinner-border spinner-border-sm mr-1"></span>}
                Submit
            </button>
            {isSubmitted && <Redirect push to='/' />}
        </form>
    );
}