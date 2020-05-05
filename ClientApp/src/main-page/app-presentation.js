import React from 'react';
import FamilyTree from '../FamilyTree'
import PersonDetails from '../PersonDetails'
import EditDetails from '../EditDetails'
import { makeStyles } from '@material-ui/core/styles';
import Header from '../Header';
import Upload from '../UploadImage';
import GetImage from '../GetImage';

const useStyles = makeStyles((theme) => ({
  root: {
    flexGrow: 1,
  },
  paper: {
    padding: theme.spacing(2),
    textAlign: 'center',
    color: theme.palette.text.secondary,
  },
}));

const renderSwitch = (props) => {
  switch(props.activeView) {
    case 'Detail':
      return <PersonDetails id={props.activePerson} selectedPerson={props.selectedPerson}/>
    case 'Edit':
      return <EditDetails id={props.activePerson}/>
    case 'Upload':
      return <Upload/>
    case 'GetImage':
      return <GetImage  image="granddadplane.jpg"/>
    default:
      return <FamilyTree id={props.activePerson} selectedPerson={props.selectedPerson}/> 
  }
}

const AppPresentation = (props) => {
  const classes = useStyles();

  if (!props.activePerson)
    return (
      <p>...Loading</p>
    );


  return (
  <div className={classes.root}>
      <Header subtitle="Family Archive" activePerson={props.activePerson} selectedPerson={props.selectedPerson} 
        activeView={props.activeView} selectedView={props.selectedView}/>
      {renderSwitch(props)}
  </div>
  );
}

export default AppPresentation; 