import React, { useState } from "react"
import { Modal, ModalFooter, ModalHeader, Button } from "reactstrap"

export const ConfirmationButton = ({ submitLabel, prompt, onSubmit, children, color }) => {
    const [modal, setModal] = useState(false);

    const toggle = () => setModal(!modal);

    return (<>
        <Button color={color || 'primary'} onClick={toggle}>{children}</Button>
        <Modal toggle={toggle} isOpen={modal}>
            <ModalHeader toggle={toggle}>{prompt}</ModalHeader>
            <ModalFooter>
                <Button color="primary" onClick={() => { onSubmit(); toggle() }}>{submitLabel}</Button>{' '}
                <Button color="secondary" onClick={toggle}>Cancel</Button>
            </ModalFooter>
        </Modal>
    </>
    );
}