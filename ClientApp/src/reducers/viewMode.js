import { ViewMode } from '../actions'

const viewMode = (state = ViewMode.SHOW_FAMILY_TREE, action) => {
  switch (action.type) {
    case 'SET_VIEW_MODE_FILTER':
      return action.filter
    default:
      return state
  }
}

export default viewMode