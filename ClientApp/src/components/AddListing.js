import React from "react";
import { useForm } from "react-hook-form";

export default function AddListing(){
    const { register, handleSubmit } = useForm({mode: 'onBlur', defaultValues: {Quantity: 1, Location: "Lithuania"}, shouldUseNativeValidation: true});
    const onSubmit = data => {
        data.Id = null;
        data.Category = null;
        data.DatePublished = Date().toLocaleString();
        console.log(JSON.stringify(data));
        fetch('test', {
          mode: 'cors',
          method: 'POST',
          headers: { 'Content-type' : 'application/json'},
          body: JSON.stringify(data),
        }).then(function(response) {
            return response;
        }).then(function(){
        });
    };

    return(
        <form onSubmit={handleSubmit(onSubmit)}>
            <div><p>Title:</p>
                <input {...register("Name", { required: "Title is required.", 
                    maxLength: {value: 20, message: "Maximum length of 20 characters exceeded."},
                    pattern: {value: /^[a-zA-Z0-9!]+$/, message: "Please enter only A-Z letters, 0-9 numbers or ! sign."} })} />
                <p>Category:</p>
                <select {...register("Category", {required: true})}>
                    <option value = "Vehicles">Vehicles</option>
                    <option value = "Apparel">Apparel</option>
                    <option value = "Electronics">Electronics</option>
                    <option value = "Entertainment">Entertainment</option>
                </select> 
            <p>Description:</p>
                <textarea {...register("Description", {
                    maxLength: { value: 200, message: "Maximum length of 500 characters exceeded."},
                    pattern: {value: /^[a-zA-Z0-9!+]+$/, message: "Please enter only A-Z letters, 0-9 numbers or !+ signs."} })} />
            <p>Quantity:</p>    
                <input type = "number" {...register("Quantity", {required: "Quantity of minimum 1 is required.", min: 1, max: 100})} />
            <p>Location:</p>
                <input {...register("Location", {required: true})} />
            <p>Images:</p>
                <input type ="file" name="temp-image" accept="image/*"{...register("ImagePath")} />
            </div>
        <input type = "submit" value="Submit listing"/>
        </form>
    );
}