import React, { useState, useContext } from "react";
import { useForm } from "react-hook-form";
import axios from "axios";
import { Redirect, useHistory } from 'react-router';
import { Modal, ModalBody, ModalHeader, Label, Row, Col, Button } from "reactstrap";
import Plants from "../garden/Plants";
import { DisplayPlant } from "../garden/DisplayPlant";
import { GardenComp } from "../garden/garden";
import { UserContext } from "../UserContext";

export default function AddListing(props) {
    const [plantModal, setPlantModal] = useState(false);
    const togglePlantModal = () => setPlantModal(!plantModal);
    const [placeModal, setPlaceModal] = useState(false);
    const togglePlaceModal = () => setPlaceModal(!placeModal);
    const { user } = useContext(UserContext);
    const [Data, setData] = useState();

    const history = useHistory();

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
        if (props.update) {
            fetchData(data);
            props.afterSubmit();
        }
        else {
            setData(data);
            setPlantModal(true);
        }
    };
    const fetchData = (data) => {
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
            if (imageAttached && response.ok) {
                axios.post('image', formData);
            }
        });
    };

    const onPlaceChosen = (x, z) => {
        setPlaceModal(false);
        const data = Object.assign({ gardenX: x, gardenZ: z }, Data);
        fetchData(data);
        history.push('/');
    }

    return (
        <div>
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
                    <label>Category</label><br />
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
                    <label>Item condition</label>
                    <select defaultValue={props.condition} className="custom-select" {...register("Condition", { required: true })}>
                        <option value="New">New</option>
                        <option value="Used">Used</option>
                        <option value="Broken">Broken</option>
                    </select>
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
                    <small className="form-text text-muted"> Radius describes how close you are located from the city center.</small>
                </div>
                <div className="form-group">
                    <label>Images</label><br />
                    <input asp-for="FileUpload.FormFile" type="file" className="form-control-file" name="temp-image" accept="image/*"{...register("ImagePath")} />
                </div><br />
                <button disabled={isSubmitting} className="btn btn-primary mr-1">
                    {isSubmitting && <span className="spinner-border spinner-border-sm mr-1"></span>}
                    Submit
                </button>
                {/* {isSubmitted && <Redirect push to='/' />} */}
            </form>
            <Modal isOpen={plantModal} toggle={togglePlantModal} >
                <ModalHeader><Label>Choose a plant to add to your garden</Label></ModalHeader>
                <ModalBody>
                    <Row>
                        {Object.entries(Plants).map(([key, p], ind) => <Col key={ind}>
                            <Button onClick={() => {
                                setData(Object.assign({ gardenPlant: key }, Data));
                                setPlantModal(false);
                                setPlaceModal(true);
                            }} style={{ display: 'flex', justifyContent: 'center', margin: 'auto' }}><DisplayPlant Plant={p} /></Button>
                        </Col>)}
                    </Row>
                </ModalBody>
            </Modal>
            <Modal isOpen={placeModal} toggle={togglePlaceModal} >
                <ModalHeader><Label>Choose a place to situate your plant</Label></ModalHeader>
                <ModalBody>
                    <GardenComp username={user.username} onPlaceChosen={onPlaceChosen} />
                </ModalBody>
            </Modal>
        </div>
    );
}