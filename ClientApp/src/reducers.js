import { combineReducers } from 'redux'
import {
  SET_VIEW_MODE_FILTER,
  ViewMode
} from './actions'
const { SHOW_FAMILY_TREE } = ViewMode

function viewMode(state = SHOW_FAMILY_TREE, action) {
  switch (action.type) {
    case SET_VIEW_MODE_FILTER:
      return action.filter
    default:
      return state
  }
}


const viewModeApp = combineReducers({
  viewMode
})

export default viewModeApp