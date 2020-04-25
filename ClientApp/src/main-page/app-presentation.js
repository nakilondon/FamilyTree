import React from 'react';
import FamilyTree from '../FamilyTree'
import PersonDetails from '../PersonDetails'
import SelectPerson from '../PersonSelect'

const AppPresentation = (props) => {

  if (!props.activePerson)
    return (
      <p>...Loading</p>
    );

  return (
    <div className="container-fluid h-100">
      <h1 id="tabelLabel" >Family Tree</h1>
      <SelectPerson id={props.activePerson} selectedPerson={props.selectedPerson}/>
      <div className="row h-100">
        <FamilyTree id={props.activePerson} selectedPerson={props.selectedPerson}/>
        <PersonDetails id={props.activePerson} selectedPerson={props.selectedPerson}/>
      </div>
    </div>
  );
}

export default AppPresentation; 