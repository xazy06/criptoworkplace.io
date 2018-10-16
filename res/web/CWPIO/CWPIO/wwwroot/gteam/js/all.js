"object"==typeof navigator&&function(e,t){"object"==typeof exports&&"undefined"!=typeof module?module.exports=t():"function"==typeof define&&define.amd?define("Plyr",t):e.Plyr=t()}(this,function(){"use strict";var e=function(e){return null!=e?e.constructor:null},t=function(e,t){return Boolean(e&&t&&e instanceof t)},i=function(e){return null==e},n=function(t){return e(t)===Object},a=function(t){return e(t)===String},s=function(e){return Array.isArray(e)},r=function(e){return t(e,NodeList)},l=function(e){return i(e)||(a(e)||s(e)||r(e))&&!e.length||n(e)&&!Object.keys(e).length},o={nullOrUndefined:i,object:n,number:function(t){return e(t)===Number&&!Number.isNaN(t)},string:a,boolean:function(t){return e(t)===Boolean},function:function(t){return e(t)===Function},array:s,weakMap:function(e){return t(e,WeakMap)},nodeList:r,element:function(e){return t(e,Element)},textNode:function(t){return e(t)===Text},event:function(e){return t(e,Event)},cue:function(e){return t(e,window.TextTrackCue)||t(e,window.VTTCue)},track:function(e){return t(e,TextTrack)||!i(e)&&a(e.kind)},url:function(e){if(t(e,window.URL))return!0;var i=e;e.startsWith("http://")&&e.startsWith("https://")||(i="http://"+e);try{return!l(new URL(i).hostname)}catch(e){return!1}},empty:l},c=function(){var e=!1;try{var t=Object.defineProperty({},"passive",{get:function(){return e=!0,null}});window.addEventListener("test",null,t),window.removeEventListener("test",null,t)}catch(e){}return e}();function u(e,t,i){var n=arguments.length>3&&void 0!==arguments[3]&&arguments[3],a=this,s=!(arguments.length>4&&void 0!==arguments[4])||arguments[4],r=arguments.length>5&&void 0!==arguments[5]&&arguments[5];if(e&&"addEventListener"in e&&!o.empty(t)&&o.function(i)){var l=t.split(" "),u=r;c&&(u={passive:s,capture:r}),l.forEach(function(t){a&&a.eventListeners&&n&&a.eventListeners.push({element:e,type:t,callback:i,options:u}),e[n?"addEventListener":"removeEventListener"](t,i,u)})}}function d(e){var t=arguments.length>1&&void 0!==arguments[1]?arguments[1]:"",i=arguments[2],n=!(arguments.length>3&&void 0!==arguments[3])||arguments[3],a=arguments.length>4&&void 0!==arguments[4]&&arguments[4];u.call(this,e,t,i,!0,n,a)}function p(e){var t=arguments.length>1&&void 0!==arguments[1]?arguments[1]:"",i=arguments[2],n=!(arguments.length>3&&void 0!==arguments[3])||arguments[3],a=arguments.length>4&&void 0!==arguments[4]&&arguments[4];u.call(this,e,t,i,!1,n,a)}function h(e){var t=arguments.length>1&&void 0!==arguments[1]?arguments[1]:"",i=arguments[2],n=!(arguments.length>3&&void 0!==arguments[3])||arguments[3],a=arguments.length>4&&void 0!==arguments[4]&&arguments[4];u.call(this,e,t,function s(){p(e,t,s,n,a);for(var r=arguments.length,l=Array(r),o=0;o<r;o++)l[o]=arguments[o];i.apply(this,l)},!0,n,a)}function m(e){var t=arguments.length>1&&void 0!==arguments[1]?arguments[1]:"",i=arguments.length>2&&void 0!==arguments[2]&&arguments[2],n=arguments.length>3&&void 0!==arguments[3]?arguments[3]:{};if(o.element(e)&&!o.empty(t)){var a=new CustomEvent(t,{bubbles:i,detail:Object.assign({},n,{plyr:this})});e.dispatchEvent(a)}}var f=function(e,t){if(!(e instanceof t))throw new TypeError("Cannot call a class as a function")},g=function(){function e(e,t){for(var i=0;i<t.length;i++){var n=t[i];n.enumerable=n.enumerable||!1,n.configurable=!0,"value"in n&&(n.writable=!0),Object.defineProperty(e,n.key,n)}}return function(t,i,n){return i&&e(t.prototype,i),n&&e(t,n),t}}(),y=function(e,t,i){return t in e?Object.defineProperty(e,t,{value:i,enumerable:!0,configurable:!0,writable:!0}):e[t]=i,e},v=function(){return function(e,t){if(Array.isArray(e))return e;if(Symbol.iterator in Object(e))return function(e,t){var i=[],n=!0,a=!1,s=void 0;try{for(var r,l=e[Symbol.iterator]();!(n=(r=l.next()).done)&&(i.push(r.value),!t||i.length!==t);n=!0);}catch(e){a=!0,s=e}finally{try{!n&&l.return&&l.return()}finally{if(a)throw s}}return i}(e,t);throw new TypeError("Invalid attempt to destructure non-iterable instance")}}();function b(e,t){var i=e.length?e:[e];Array.from(i).reverse().forEach(function(e,i){var n=i>0?t.cloneNode(!0):t,a=e.parentNode,s=e.nextSibling;n.appendChild(e),s?a.insertBefore(n,s):a.appendChild(n)})}function k(e,t){o.element(e)&&!o.empty(t)&&Object.entries(t).filter(function(e){var t=v(e,2)[1];return!o.nullOrUndefined(t)}).forEach(function(t){var i=v(t,2),n=i[0],a=i[1];return e.setAttribute(n,a)})}function w(e,t,i){var n=document.createElement(e);return o.object(t)&&k(n,t),o.string(i)&&(n.innerText=i),n}function T(e,t,i,n){t.appendChild(w(e,i,n))}function A(e){o.nodeList(e)||o.array(e)?Array.from(e).forEach(A):o.element(e)&&o.element(e.parentNode)&&e.parentNode.removeChild(e)}function C(e){for(var t=e.childNodes.length;t>0;)e.removeChild(e.lastChild),t-=1}function E(e,t){return o.element(t)&&o.element(t.parentNode)&&o.element(e)?(t.parentNode.replaceChild(e,t),e):null}function S(e,t){if(!o.string(e)||o.empty(e))return{};var i={},n=t;return e.split(",").forEach(function(e){var t=e.trim(),a=t.replace(".",""),s=t.replace(/[[\]]/g,"").split("="),r=s[0],l=s.length>1?s[1].replace(/["']/g,""):"";switch(t.charAt(0)){case".":o.object(n)&&o.string(n.class)&&(n.class+=" "+a),i.class=a;break;case"#":i.id=t.replace("#","");break;case"[":i[r]=l}}),i}function P(e,t){if(o.element(e)){var i=t;o.boolean(i)||(i=!e.hasAttribute("hidden")),i?e.setAttribute("hidden",""):e.removeAttribute("hidden")}}function N(e,t,i){if(o.element(e)){var n="toggle";return void 0!==i&&(n=i?"add":"remove"),e.classList[n](t),e.classList.contains(t)}return null}function L(e,t){return o.element(e)&&e.classList.contains(t)}function M(e,t){var i={Element:Element};return(i.matches||i.webkitMatchesSelector||i.mozMatchesSelector||i.msMatchesSelector||function(){return Array.from(document.querySelectorAll(t)).includes(this)}).call(e,t)}function x(e){return this.elements.container.querySelectorAll(e)}function _(e){return this.elements.container.querySelector(e)}function q(){var e=document.activeElement;return e=e&&e!==document.body?document.querySelector(":focus"):null}var I,O,j,R=(I=document.createElement("span"),O={WebkitTransition:"webkitTransitionEnd",MozTransition:"transitionend",OTransition:"oTransitionEnd otransitionend",transition:"transitionend"},j=Object.keys(O).find(function(e){return void 0!==I.style[e]}),!!o.string(j)&&O[j]);var H,V={isIE:!!document.documentMode,isWebkit:"WebkitAppearance"in document.documentElement.style&&!/Edge/.test(navigator.userAgent),isIPhone:/(iPhone|iPod)/gi.test(navigator.platform),isIos:/(iPad|iPhone|iPod)/gi.test(navigator.platform)},B={"audio/ogg":"vorbis","audio/wav":"1","video/webm":"vp8, vorbis","video/mp4":"avc1.42E01E, mp4a.40.2","video/ogg":"theora"},D={audio:"canPlayType"in document.createElement("audio"),video:"canPlayType"in document.createElement("video"),check:function(e,t,i){var n=V.isIPhone&&i&&D.playsinline,a=D[e]||"html5"!==t;return{api:a,ui:a&&D.rangeInput&&("video"!==e||!V.isIPhone||n)}},pip:!V.isIPhone&&o.function(w("video").webkitSetPresentationMode),airplay:o.function(window.WebKitPlaybackTargetAvailabilityEvent),playsinline:"playsInline"in document.createElement("video"),mime:function(e){var t=e.split("/"),i=v(t,1)[0];if(!this.isHTML5||i!==this.type)return!1;var n=void 0;e&&e.includes("codecs=")?n=e:"audio/mpeg"===e?n="audio/mpeg;":e in B&&(n=e+'; codecs="'+B[e]+'"');try{return Boolean(n&&this.media.canPlayType(n).replace(/no/,""))}catch(e){return!1}},textTracks:"textTracks"in document.createElement("video"),rangeInput:(H=document.createElement("input"),H.type="range","range"===H.type),touch:"ontouchstart"in document.documentElement,transitions:!1!==R,reducedMotion:"matchMedia"in window&&window.matchMedia("(prefers-reduced-motion)").matches},F={getSources:function(){var e=this;return this.isHTML5?Array.from(this.media.querySelectorAll("source")).filter(function(t){return D.mime.call(e,t.getAttribute("type"))}):[]},getQualityOptions:function(){return F.getSources.call(this).map(function(e){return Number(e.getAttribute("size"))}).filter(Boolean)},extend:function(){if(this.isHTML5){var e=this;Object.defineProperty(e.media,"quality",{get:function(){var t=F.getSources.call(e).find(function(t){return t.getAttribute("src")===e.source});return t&&Number(t.getAttribute("size"))},set:function(t){var i=F.getSources.call(e).find(function(e){return Number(e.getAttribute("size"))===t});if(i){var n=e.media,a=n.currentTime,s=n.paused,r=n.preload,l=n.readyState;e.media.src=i.getAttribute("src"),("none"!==r||l)&&(e.once("loadedmetadata",function(){e.currentTime=a,s||e.play()}),e.media.load()),m.call(e,e.media,"qualitychange",!1,{quality:t})}}})}},cancelRequests:function(){this.isHTML5&&(A(F.getSources.call(this)),this.media.setAttribute("src",this.config.blankVideo),this.media.load(),this.debug.log("Cancelled network requests"))}};function U(e,t){return t.split(".").reduce(function(e,t){return e&&e[t]},e)}function z(){for(var e=arguments.length>0&&void 0!==arguments[0]?arguments[0]:{},t=arguments.length,i=Array(t>1?t-1:0),n=1;n<t;n++)i[n-1]=arguments[n];if(!i.length)return e;var a=i.shift();return o.object(a)?(Object.keys(a).forEach(function(t){o.object(a[t])?(Object.keys(e).includes(t)||Object.assign(e,y({},t,{})),z(e[t],a[t])):Object.assign(e,y({},t,a[t]))}),z.apply(void 0,[e].concat(i))):e}function W(e){for(var t=arguments.length,i=Array(t>1?t-1:0),n=1;n<t;n++)i[n-1]=arguments[n];return o.empty(e)?e:e.toString().replace(/{(\d+)}/g,function(e,t){return i[t].toString()})}function K(){var e=arguments.length>0&&void 0!==arguments[0]?arguments[0]:"",t=arguments.length>1&&void 0!==arguments[1]?arguments[1]:"",i=arguments.length>2&&void 0!==arguments[2]?arguments[2]:"";return e.replace(new RegExp(t.toString().replace(/([.*+?^=!:${}()|[\]/\\])/g,"\\$1"),"g"),i.toString())}function Y(){return(arguments.length>0&&void 0!==arguments[0]?arguments[0]:"").toString().replace(/\w\S*/g,function(e){return e.charAt(0).toUpperCase()+e.substr(1).toLowerCase()})}function Q(){var e=(arguments.length>0&&void 0!==arguments[0]?arguments[0]:"").toString();return(e=function(){var e=(arguments.length>0&&void 0!==arguments[0]?arguments[0]:"").toString();return e=K(e,"-"," "),e=K(e,"_"," "),K(e=Y(e)," ","")}(e)).charAt(0).toLowerCase()+e.slice(1)}function J(e){var t=document.createElement("div");return t.appendChild(e),t.innerHTML}var $=function(){var e=arguments.length>0&&void 0!==arguments[0]?arguments[0]:"",t=arguments.length>1&&void 0!==arguments[1]?arguments[1]:{};if(o.empty(e)||o.empty(t))return"";var i=U(t.i18n,e);if(o.empty(i))return"";var n={"{seektime}":t.seekTime,"{title}":t.title};return Object.entries(n).forEach(function(e){var t=v(e,2),n=t[0],a=t[1];i=K(i,n,a)}),i};function G(e){return o.array(e)?e.filter(function(t,i){return e.indexOf(t)===i}):e}var X=function(){function e(t){f(this,e),this.enabled=t.config.storage.enabled,this.key=t.config.storage.key}return g(e,[{key:"get",value:function(t){if(!e.supported||!this.enabled)return null;var i=window.localStorage.getItem(this.key);if(o.empty(i))return null;var n=JSON.parse(i);return o.string(t)&&t.length?n[t]:n}},{key:"set",value:function(t){if(e.supported&&this.enabled&&o.object(t)){var i=this.get();o.empty(i)&&(i={}),z(i,t),window.localStorage.setItem(this.key,JSON.stringify(i))}}}],[{key:"supported",get:function(){try{if(!("localStorage"in window))return!1;return window.localStorage.setItem("___test","___test"),window.localStorage.removeItem("___test"),!0}catch(e){return!1}}}]),e}();function Z(e){var t=arguments.length>1&&void 0!==arguments[1]?arguments[1]:"text";return new Promise(function(i,n){try{var a=new XMLHttpRequest;if(!("withCredentials"in a))return;a.addEventListener("load",function(){if("text"===t)try{i(JSON.parse(a.responseText))}catch(e){i(a.responseText)}else i(a.response)}),a.addEventListener("error",function(){throw new Error(a.status)}),a.open("GET",e,!0),a.responseType=t,a.send()}catch(e){n(e)}})}function ee(e,t){if(o.string(e)){var i=o.string(t),n=function(){return null!==document.getElementById(t)},a=function(e,t){e.innerHTML=t,i&&n()||document.body.insertAdjacentElement("afterbegin",e)};if(!i||!n()){var s=X.supported,r=document.createElement("div");if(r.setAttribute("hidden",""),i&&r.setAttribute("id",t),s){var l=window.localStorage.getItem("cache-"+t);if(null!==l){var c=JSON.parse(l);a(r,c.content)}}Z(e).then(function(e){o.empty(e)||(s&&window.localStorage.setItem("cache-"+t,JSON.stringify({content:e})),a(r,e))}).catch(function(){})}}}var te=function(e){return parseInt(e/60/60%60,10)},ie=function(e){return parseInt(e/60%60,10)},ne=function(e){return parseInt(e%60,10)};function ae(){var e=arguments.length>0&&void 0!==arguments[0]?arguments[0]:0,t=arguments.length>1&&void 0!==arguments[1]&&arguments[1],i=arguments.length>2&&void 0!==arguments[2]&&arguments[2];if(!o.number(e))return ae(null,t,i);var n=function(e){return("0"+e).slice(-2)},a=te(e),s=ie(e),r=ne(e);return t||a>0?a+=":":a="",(i&&e>0?"-":"")+a+n(s)+":"+n(r)}var se={getIconUrl:function(){var e=new URL(this.config.iconUrl,window.location).host!==window.location.host||V.isIE&&!window.svg4everybody;return{url:this.config.iconUrl,cors:e}},findElements:function(){try{return this.elements.controls=_.call(this,this.config.selectors.controls.wrapper),this.elements.buttons={play:x.call(this,this.config.selectors.buttons.play),pause:_.call(this,this.config.selectors.buttons.pause),restart:_.call(this,this.config.selectors.buttons.restart),rewind:_.call(this,this.config.selectors.buttons.rewind),fastForward:_.call(this,this.config.selectors.buttons.fastForward),mute:_.call(this,this.config.selectors.buttons.mute),pip:_.call(this,this.config.selectors.buttons.pip),airplay:_.call(this,this.config.selectors.buttons.airplay),settings:_.call(this,this.config.selectors.buttons.settings),captions:_.call(this,this.config.selectors.buttons.captions),fullscreen:_.call(this,this.config.selectors.buttons.fullscreen)},this.elements.progress=_.call(this,this.config.selectors.progress),this.elements.inputs={seek:_.call(this,this.config.selectors.inputs.seek),volume:_.call(this,this.config.selectors.inputs.volume)},this.elements.display={buffer:_.call(this,this.config.selectors.display.buffer),currentTime:_.call(this,this.config.selectors.display.currentTime),duration:_.call(this,this.config.selectors.display.duration)},o.element(this.elements.progress)&&(this.elements.display.seekTooltip=this.elements.progress.querySelector("."+this.config.classNames.tooltip)),!0}catch(e){return this.debug.warn("It looks like there is a problem with your custom controls HTML",e),this.toggleNativeControls(!0),!1}},createIcon:function(e,t){var i=se.getIconUrl.call(this),n=(i.cors?"":i.url)+"#"+this.config.iconPrefix,a=document.createElementNS("http://www.w3.org/2000/svg","svg");k(a,z(t,{role:"presentation",focusable:"false"}));var s=document.createElementNS("http://www.w3.org/2000/svg","use"),r=n+"-"+e;return"href"in s?s.setAttributeNS("http://www.w3.org/1999/xlink","href",r):s.setAttributeNS("http://www.w3.org/1999/xlink","xlink:href",r),a.appendChild(s),a},createLabel:function(e){var t=arguments.length>1&&void 0!==arguments[1]?arguments[1]:{},i={pip:"PIP",airplay:"AirPlay"}[e]||$(e,this.config);return w("span",Object.assign({},t,{class:[t.class,this.config.classNames.hidden].filter(Boolean).join(" ")}),i)},createBadge:function(e){if(o.empty(e))return null;var t=w("span",{class:this.config.classNames.menu.value});return t.appendChild(w("span",{class:this.config.classNames.menu.badge},e)),t},createButton:function(e,t){var i=w("button"),n=Object.assign({},t),a=Q(e),s=!1,r=void 0,l=void 0,c=void 0,u=void 0;switch("type"in n||(n.type="button"),"class"in n?n.class.includes(this.config.classNames.control)||(n.class+=" "+this.config.classNames.control):n.class=this.config.classNames.control,e){case"play":s=!0,r="play",c="pause",l="play",u="pause";break;case"mute":s=!0,r="mute",c="unmute",l="volume",u="muted";break;case"captions":s=!0,r="enableCaptions",c="disableCaptions",l="captions-off",u="captions-on";break;case"fullscreen":s=!0,r="enterFullscreen",c="exitFullscreen",l="enter-fullscreen",u="exit-fullscreen";break;case"play-large":n.class+=" "+this.config.classNames.control+"--overlaid",a="play",r="play",l="play";break;default:r=a,l=e}s?(i.appendChild(se.createIcon.call(this,u,{class:"icon--pressed"})),i.appendChild(se.createIcon.call(this,l,{class:"icon--not-pressed"})),i.appendChild(se.createLabel.call(this,c,{class:"label--pressed"})),i.appendChild(se.createLabel.call(this,r,{class:"label--not-pressed"}))):(i.appendChild(se.createIcon.call(this,l)),i.appendChild(se.createLabel.call(this,r))),z(n,S(this.config.selectors.buttons[a],n)),k(i,n),"play"===a?(o.array(this.elements.buttons[a])||(this.elements.buttons[a]=[]),this.elements.buttons[a].push(i)):this.elements.buttons[a]=i;var d=this.config.classNames.controlPressed;return Object.defineProperty(i,"pressed",{enumerable:!0,get:function(){return L(i,d)},set:function(){var e=arguments.length>0&&void 0!==arguments[0]&&arguments[0];N(i,d,e)}}),i},createRange:function(e,t){var i=w("input",z(S(this.config.selectors.inputs[e]),{type:"range",min:0,max:100,step:.01,value:0,autocomplete:"off",role:"slider","aria-label":$(e,this.config),"aria-valuemin":0,"aria-valuemax":100,"aria-valuenow":0},t));return this.elements.inputs[e]=i,se.updateRangeFill.call(this,i),i},createProgress:function(e,t){var i=w("progress",z(S(this.config.selectors.display[e]),{min:0,max:100,value:0,role:"presentation","aria-hidden":!0},t));if("volume"!==e){i.appendChild(w("span",null,"0"));var n={played:"played",buffer:"buffered"}[e],a=n?$(n,this.config):"";i.innerText="% "+a.toLowerCase()}return this.elements.display[e]=i,i},createTime:function(e){var t=S(this.config.selectors.display[e]),i=w("div",z(t,{class:"plyr__time "+t.class,"aria-label":$(e,this.config)}),"00:00");return this.elements.display[e]=i,i},createMenuItem:function(e){var t=e.value,i=e.list,n=e.type,a=e.title,s=e.badge,r=void 0===s?null:s,l=e.checked,c=void 0!==l&&l,u=w("li"),d=w("label",{class:this.config.classNames.control}),p=w("input",z(S(this.config.selectors.inputs[n]),{type:"radio",name:"plyr-"+n,value:t,checked:c,class:"plyr__sr-only"})),h=w("span",{hidden:""});d.appendChild(p),d.appendChild(h),d.insertAdjacentHTML("beforeend",a),o.element(r)&&d.appendChild(r),u.appendChild(d),i.appendChild(u)},formatTime:function(){var e=arguments.length>0&&void 0!==arguments[0]?arguments[0]:0,t=arguments.length>1&&void 0!==arguments[1]&&arguments[1];return o.number(e)?ae(e,te(this.duration)>0,t):e},updateTimeDisplay:function(){var e=arguments.length>0&&void 0!==arguments[0]?arguments[0]:null,t=arguments.length>1&&void 0!==arguments[1]?arguments[1]:0,i=arguments.length>2&&void 0!==arguments[2]&&arguments[2];o.element(e)&&o.number(t)&&(e.innerText=se.formatTime(t,i))},updateVolume:function(){this.supported.ui&&(o.element(this.elements.inputs.volume)&&se.setRange.call(this,this.elements.inputs.volume,this.muted?0:this.volume),o.element(this.elements.buttons.mute)&&(this.elements.buttons.mute.pressed=this.muted||0===this.volume))},setRange:function(e){var t=arguments.length>1&&void 0!==arguments[1]?arguments[1]:0;o.element(e)&&(e.value=t,se.updateRangeFill.call(this,e))},updateProgress:function(e){var t=this;if(this.supported.ui&&o.event(e)){var i,n,a=0;if(e)switch(e.type){case"timeupdate":case"seeking":case"seeked":i=this.currentTime,n=this.duration,a=0===i||0===n||Number.isNaN(i)||Number.isNaN(n)?0:(i/n*100).toFixed(2),"timeupdate"===e.type&&se.setRange.call(this,this.elements.inputs.seek,a);break;case"playing":case"progress":!function(e,i){var n=o.number(i)?i:0,a=o.element(e)?e:t.elements.display.buffer;if(o.element(a)){a.value=n;var s=a.getElementsByTagName("span")[0];o.element(s)&&(s.childNodes[0].nodeValue=n)}}(this.elements.display.buffer,100*this.buffered)}}},updateRangeFill:function(e){var t=o.event(e)?e.target:e;if(o.element(t)&&"range"===t.getAttribute("type")){if(M(t,this.config.selectors.inputs.seek)){t.setAttribute("aria-valuenow",this.currentTime);var i=se.formatTime(this.currentTime),n=se.formatTime(this.duration),a=$("seekLabel",this.config);t.setAttribute("aria-valuetext",a.replace("{currentTime}",i).replace("{duration}",n))}else if(M(t,this.config.selectors.inputs.volume)){var s=100*t.value;t.setAttribute("aria-valuenow",s),t.setAttribute("aria-valuetext",s+"%")}else t.setAttribute("aria-valuenow",t.value);V.isWebkit&&t.style.setProperty("--value",t.value/t.max*100+"%")}},updateSeekTooltip:function(e){var t=this;if(this.config.tooltips.seek&&o.element(this.elements.inputs.seek)&&o.element(this.elements.display.seekTooltip)&&0!==this.duration){var i=0,n=this.elements.progress.getBoundingClientRect(),a=this.config.classNames.tooltip+"--visible",s=function(e){N(t.elements.display.seekTooltip,a,e)};if(this.touch)s(!1);else{if(o.event(e))i=100/n.width*(e.pageX-n.left);else{if(!L(this.elements.display.seekTooltip,a))return;i=parseFloat(this.elements.display.seekTooltip.style.left,10)}i<0?i=0:i>100&&(i=100),se.updateTimeDisplay.call(this,this.elements.display.seekTooltip,this.duration/100*i),this.elements.display.seekTooltip.style.left=i+"%",o.event(e)&&["mouseenter","mouseleave"].includes(e.type)&&s("mouseenter"===e.type)}}},timeUpdate:function(e){var t=!o.element(this.elements.display.duration)&&this.config.invertTime;se.updateTimeDisplay.call(this,this.elements.display.currentTime,t?this.duration-this.currentTime:this.currentTime,t),e&&"timeupdate"===e.type&&this.media.seeking||se.updateProgress.call(this,e)},durationUpdate:function(){if(this.supported.ui&&(this.config.invertTime||!this.currentTime)){if(this.duration>=Math.pow(2,32))return P(this.elements.display.currentTime,!0),void P(this.elements.progress,!0);o.element(this.elements.inputs.seek)&&this.elements.inputs.seek.setAttribute("aria-valuemax",this.duration);var e=o.element(this.elements.display.duration);!e&&this.config.displayDuration&&this.paused&&se.updateTimeDisplay.call(this,this.elements.display.currentTime,this.duration),e&&se.updateTimeDisplay.call(this,this.elements.display.duration,this.duration),se.updateSeekTooltip.call(this)}},toggleTab:function(e,t){P(this.elements.settings.tabs[e],!t)},setQualityMenu:function(e){var t=this;if(o.element(this.elements.settings.panes.quality)){var i=this.elements.settings.panes.quality.querySelector("ul");o.array(e)&&(this.options.quality=G(e).filter(function(e){return t.config.quality.options.includes(e)}));var n=!o.empty(this.options.quality)&&this.options.quality.length>1;if(se.toggleTab.call(this,"quality",n),se.checkMenu.call(this),n){C(i);this.options.quality.sort(function(e,i){var n=t.config.quality.options;return n.indexOf(e)>n.indexOf(i)?1:-1}).forEach(function(e){se.createMenuItem.call(t,{value:e,list:i,type:"quality",title:se.getLabel.call(t,"quality",e),badge:function(e){var i=$("qualityBadge."+e,t.config);return i.length?se.createBadge.call(t,i):null}(e)})}),se.updateSetting.call(this,"quality",i)}}},getLabel:function(e,t){switch(e){case"speed":return 1===t?$("normal",this.config):t+"&times;";case"quality":if(o.number(t)){var i=$("qualityLabel."+t,this.config);return i.length?i:t+"p"}return Y(t);case"captions":return oe.getLabel.call(this);default:return null}},updateSetting:function(e,t,i){var n=this.elements.settings.panes[e],a=null,s=t;if("captions"===e)a=this.currentTrack;else{if(a=o.empty(i)?this[e]:i,o.empty(a)&&(a=this.config[e].default),!o.empty(this.options[e])&&!this.options[e].includes(a))return void this.debug.warn("Unsupported value of '"+a+"' for "+e);if(!this.config[e].options.includes(a))return void this.debug.warn("Disabled value of '"+a+"' for "+e)}if(o.element(s)||(s=n&&n.querySelector("ul")),o.element(s)){this.elements.settings.tabs[e].querySelector("."+this.config.classNames.menu.value).innerHTML=se.getLabel.call(this,e,a);var r=s&&s.querySelector('input[value="'+a+'"]');o.element(r)&&(r.checked=!0)}},setCaptionsMenu:function(){var e=this,t=this.elements.settings.panes.captions.querySelector("ul"),i=oe.getTracks.call(this);if(se.toggleTab.call(this,"captions",i.length),C(t),se.checkMenu.call(this),i.length){var n=i.map(function(i,n){return{value:n,checked:e.captions.toggled&&e.currentTrack===n,title:oe.getLabel.call(e,i),badge:i.language&&se.createBadge.call(e,i.language.toUpperCase()),list:t,type:"language"}});n.unshift({value:-1,checked:!this.captions.toggled,title:$("disabled",this.config),list:t,type:"language"}),n.forEach(se.createMenuItem.bind(this)),se.updateSetting.call(this,"captions",t)}},setSpeedMenu:function(e){var t=this;if(this.config.controls.includes("settings")&&this.config.settings.includes("speed")&&o.element(this.elements.settings.panes.speed)){o.array(e)?this.options.speed=e:(this.isHTML5||this.isVimeo)&&(this.options.speed=[.5,.75,1,1.25,1.5,1.75,2]),this.options.speed=this.options.speed.filter(function(e){return t.config.speed.options.includes(e)});var i=!o.empty(this.options.speed)&&this.options.speed.length>1;if(se.toggleTab.call(this,"speed",i),se.checkMenu.call(this),i){var n=this.elements.settings.panes.speed.querySelector("ul");C(n),this.options.speed.forEach(function(e){se.createMenuItem.call(t,{value:e,list:n,type:"speed",title:se.getLabel.call(t,"speed",e)})}),se.updateSetting.call(this,"speed",n)}}},checkMenu:function(){var e=this.elements.settings.tabs,t=!o.empty(e)&&Object.values(e).some(function(e){return!e.hidden});P(this.elements.settings.menu,!t)},toggleMenu:function(e){var t=this.elements.settings.form,i=this.elements.buttons.settings;if(o.element(t)&&o.element(i)){var n=o.boolean(e)?e:o.element(t)&&t.hasAttribute("hidden");if(o.event(e)){var a=o.element(t)&&t.contains(e.target),s=e.target===this.elements.buttons.settings;if(a||!a&&!s&&n)return;s&&e.stopPropagation()}o.element(i)&&i.setAttribute("aria-expanded",n),o.element(t)&&(P(t,!n),N(this.elements.container,this.config.classNames.menu.open,n),n?t.removeAttribute("tabindex"):t.setAttribute("tabindex",-1))}},getTabSize:function(e){var t=e.cloneNode(!0);t.style.position="absolute",t.style.opacity=0,t.removeAttribute("hidden"),Array.from(t.querySelectorAll("input[name]")).forEach(function(e){var t=e.getAttribute("name");e.setAttribute("name",t+"-clone")}),e.parentNode.appendChild(t);var i=t.scrollWidth,n=t.scrollHeight;return A(t),{width:i,height:n}},showTab:function(){var e=this,t=arguments.length>0&&void 0!==arguments[0]?arguments[0]:"",i=this.elements.settings.menu,n=document.getElementById(t);if(o.element(n)&&"tabpanel"===n.getAttribute("role")){var a=i.querySelector('[role="tabpanel"]:not([hidden])'),s=a.parentNode;if(Array.from(i.querySelectorAll('[aria-controls="'+a.getAttribute("id")+'"]')).forEach(function(e){e.setAttribute("aria-expanded",!1)}),D.transitions&&!D.reducedMotion){s.style.width=a.scrollWidth+"px",s.style.height=a.scrollHeight+"px";var r=se.getTabSize.call(this,n);d.call(this,s,R,function t(i){i.target===s&&["width","height"].includes(i.propertyName)&&(s.style.width="",s.style.height="",p.call(e,s,R,t))}),s.style.width=r.width+"px",s.style.height=r.height+"px"}P(a,!0),a.setAttribute("tabindex",-1),P(n,!1);var l=x.call(this,'[aria-controls="'+t+'"]');Array.from(l).forEach(function(e){e.setAttribute("aria-expanded",!0)}),n.removeAttribute("tabindex"),n.querySelectorAll("button:not(:disabled), input:not(:disabled), [tabindex]")[0].focus()}},create:function(e){var t=this;if(o.empty(this.config.controls))return null;var i=w("div",S(this.config.selectors.controls.wrapper));if(this.config.controls.includes("restart")&&i.appendChild(se.createButton.call(this,"restart")),this.config.controls.includes("rewind")&&i.appendChild(se.createButton.call(this,"rewind")),this.config.controls.includes("play")&&i.appendChild(se.createButton.call(this,"play")),this.config.controls.includes("fast-forward")&&i.appendChild(se.createButton.call(this,"fast-forward")),this.config.controls.includes("progress")){var n=w("div",S(this.config.selectors.progress));if(n.appendChild(se.createRange.call(this,"seek",{id:"plyr-seek-"+e.id})),n.appendChild(se.createProgress.call(this,"buffer")),this.config.tooltips.seek){var a=w("span",{class:this.config.classNames.tooltip},"00:00");n.appendChild(a),this.elements.display.seekTooltip=a}this.elements.progress=n,i.appendChild(this.elements.progress)}if(this.config.controls.includes("current-time")&&i.appendChild(se.createTime.call(this,"currentTime")),this.config.controls.includes("duration")&&i.appendChild(se.createTime.call(this,"duration")),this.config.controls.includes("mute")&&i.appendChild(se.createButton.call(this,"mute")),this.config.controls.includes("volume")){var s=w("div",{class:"plyr__volume"}),r={max:1,step:.05,value:this.config.volume};s.appendChild(se.createRange.call(this,"volume",z(r,{id:"plyr-volume-"+e.id}))),this.elements.volume=s,i.appendChild(s)}if(this.config.controls.includes("captions")&&i.appendChild(se.createButton.call(this,"captions")),this.config.controls.includes("settings")&&!o.empty(this.config.settings)){var l=w("div",{class:"plyr__menu",hidden:""});l.appendChild(se.createButton.call(this,"settings",{id:"plyr-settings-toggle-"+e.id,"aria-haspopup":!0,"aria-controls":"plyr-settings-"+e.id,"aria-expanded":!1}));var c=w("form",{class:"plyr__menu__container",id:"plyr-settings-"+e.id,hidden:"","aria-labelled-by":"plyr-settings-toggle-"+e.id,role:"tablist",tabindex:-1}),u=w("div"),d=w("div",{id:"plyr-settings-"+e.id+"-home","aria-labelled-by":"plyr-settings-toggle-"+e.id,role:"tabpanel"}),p=w("ul",{role:"tablist"});this.config.settings.forEach(function(i){var n=w("li",{role:"tab",hidden:""}),a=w("button",z(S(t.config.selectors.buttons.settings),{type:"button",class:t.config.classNames.control+" "+t.config.classNames.control+"--forward",id:"plyr-settings-"+e.id+"-"+i+"-tab","aria-haspopup":!0,"aria-controls":"plyr-settings-"+e.id+"-"+i,"aria-expanded":!1}),$(i,t.config)),s=w("span",{class:t.config.classNames.menu.value});s.innerHTML=e[i],a.appendChild(s),n.appendChild(a),p.appendChild(n),t.elements.settings.tabs[i]=n}),d.appendChild(p),u.appendChild(d),this.config.settings.forEach(function(i){var n=w("div",{id:"plyr-settings-"+e.id+"-"+i,hidden:"","aria-labelled-by":"plyr-settings-"+e.id+"-"+i+"-tab",role:"tabpanel",tabindex:-1}),a=w("button",{type:"button",class:t.config.classNames.control+" "+t.config.classNames.control+"--back","aria-haspopup":!0,"aria-controls":"plyr-settings-"+e.id+"-home","aria-expanded":!1},$(i,t.config));n.appendChild(a);var s=w("ul");n.appendChild(s),u.appendChild(n),t.elements.settings.panes[i]=n}),c.appendChild(u),l.appendChild(c),i.appendChild(l),this.elements.settings.form=c,this.elements.settings.menu=l}return this.config.controls.includes("pip")&&D.pip&&i.appendChild(se.createButton.call(this,"pip")),this.config.controls.includes("airplay")&&D.airplay&&i.appendChild(se.createButton.call(this,"airplay")),this.config.controls.includes("fullscreen")&&i.appendChild(se.createButton.call(this,"fullscreen")),this.config.controls.includes("play-large")&&this.elements.container.appendChild(se.createButton.call(this,"play-large")),this.elements.controls=i,this.isHTML5&&se.setQualityMenu.call(this,F.getQualityOptions.call(this)),se.setSpeedMenu.call(this),i},inject:function(){var e=this;if(this.config.loadSprite){var t=se.getIconUrl.call(this);t.cors&&ee(t.url,"sprite-plyr")}this.id=Math.floor(1e4*Math.random());var i=null;this.elements.controls=null;var n={id:this.id,seektime:this.config.seekTime,title:this.config.title},a=!0;o.string(this.config.controls)||o.element(this.config.controls)?i=this.config.controls:o.function(this.config.controls)?i=this.config.controls.call(this,n):(i=se.create.call(this,{id:this.id,seektime:this.config.seekTime,speed:this.speed,quality:this.quality,captions:oe.getLabel.call(this)}),a=!1);var s=function(e){var t=e;return Object.entries(n).forEach(function(e){var i=v(e,2),n=i[0],a=i[1];t=K(t,"{"+n+"}",a)}),t};a&&(o.string(this.config.controls)?i=s(i):o.element(i)&&(i.innerHTML=s(i.innerHTML)));var r,l=void 0;if(o.string(this.config.selectors.controls.container)&&(l=document.querySelector(this.config.selectors.controls.container)),o.element(l)||(l=this.elements.container),o.element(i)?l.appendChild(i):i&&l.insertAdjacentHTML("beforeend",i),o.element(this.elements.controls)||se.findElements.call(this),window.navigator.userAgent.includes("Edge")&&(r=l,setTimeout(function(){P(r,!0),r.offsetHeight,P(r,!1)},0)),this.config.tooltips.controls){var c=this.config,u=c.classNames,d=c.selectors,p=d.controls.wrapper+" "+d.labels+" ."+u.hidden,h=x.call(this,p);Array.from(h).forEach(function(t){N(t,e.config.classNames.hidden,!1),N(t,e.config.classNames.tooltip,!0)})}}};function re(e){var t=e;if(!(arguments.length>1&&void 0!==arguments[1])||arguments[1]){var i=document.createElement("a");i.href=t,t=i.href}try{return new URL(t)}catch(e){return null}}function le(e){var t=new URLSearchParams;return o.object(e)&&Object.entries(e).forEach(function(e){var i=v(e,2),n=i[0],a=i[1];t.set(n,a)}),t}var oe={setup:function(){if(this.supported.ui)if(!this.isVideo||this.isYouTube||this.isHTML5&&!D.textTracks)o.array(this.config.controls)&&this.config.controls.includes("settings")&&this.config.settings.includes("captions")&&se.setCaptionsMenu.call(this);else{var e,t;if(o.element(this.elements.captions)||(this.elements.captions=w("div",S(this.config.selectors.captions)),e=this.elements.captions,(t=this.elements.wrapper).parentNode.insertBefore(e,t.nextSibling)),V.isIE&&window.URL){var i=this.media.querySelectorAll("track");Array.from(i).forEach(function(e){var t=e.getAttribute("src"),i=re(t);null!==i&&i.hostname!==window.location.href.hostname&&["http:","https:"].includes(i.protocol)&&Z(t,"blob").then(function(t){e.setAttribute("src",window.URL.createObjectURL(t))}).catch(function(){A(e)})})}var n=G(Array.from(navigator.languages||navigator.language||navigator.userLanguage).map(function(e){return e.split("-")[0]})),a=(this.storage.get("language")||this.config.captions.language||"auto").toLowerCase();if("auto"===a)a=v(n,1)[0];var s=this.storage.get("captions");if(o.boolean(s)||(s=this.config.captions.active),Object.assign(this.captions,{toggled:!1,active:s,language:a,languages:n}),this.isHTML5){var r=this.config.captions.update?"addtrack removetrack":"removetrack";d.call(this,this.media.textTracks,r,oe.update.bind(this))}setTimeout(oe.update.bind(this),0)}},update:function(){var e=this,t=oe.getTracks.call(this,!0),i=this.captions,n=i.active,a=i.language,s=i.meta,r=i.currentTrackNode,l=Boolean(t.find(function(e){return e.language===a}));this.isHTML5&&this.isVideo&&t.filter(function(e){return!s.get(e)}).forEach(function(t){e.debug.log("Track added",t),s.set(t,{default:"showing"===t.mode}),t.mode="hidden",d.call(e,t,"cuechange",function(){return oe.updateCues.call(e)})}),(l&&this.language!==a||!t.includes(r))&&(oe.setLanguage.call(this,a),oe.toggle.call(this,n&&l)),N(this.elements.container,this.config.classNames.captions.enabled,!o.empty(t)),(this.config.controls||[]).includes("settings")&&this.config.settings.includes("captions")&&se.setCaptionsMenu.call(this)},toggle:function(e){var t=!(arguments.length>1&&void 0!==arguments[1])||arguments[1];if(this.supported.ui){var i=this.captions.toggled,n=this.config.classNames.captions.active,a=o.nullOrUndefined(e)?!i:e;if(a!==i){if(t||(this.captions.active=a,this.storage.set({captions:a})),!this.language&&a&&!t){var s=oe.getTracks.call(this),r=oe.findTrack.call(this,[this.captions.language].concat(function(e){if(Array.isArray(e)){for(var t=0,i=Array(e.length);t<e.length;t++)i[t]=e[t];return i}return Array.from(e)}(this.captions.languages)),!0);return this.captions.language=r.language,void oe.set.call(this,s.indexOf(r))}this.elements.buttons.captions&&(this.elements.buttons.captions.pressed=a),N(this.elements.container,n,a),this.captions.toggled=a,se.updateSetting.call(this,"captions"),m.call(this,this.media,a?"captionsenabled":"captionsdisabled")}}},set:function(e){var t=!(arguments.length>1&&void 0!==arguments[1])||arguments[1],i=oe.getTracks.call(this);if(-1!==e)if(o.number(e))if(e in i){if(this.captions.currentTrack!==e){this.captions.currentTrack=e;var n=i[e],a=(n||{}).language;this.captions.currentTrackNode=n,se.updateSetting.call(this,"captions"),t||(this.captions.language=a,this.storage.set({language:a})),this.isVimeo&&this.embed.enableTextTrack(a),m.call(this,this.media,"languagechange")}oe.toggle.call(this,!0,t),this.isHTML5&&this.isVideo&&oe.updateCues.call(this)}else this.debug.warn("Track not found",e);else this.debug.warn("Invalid caption argument",e);else oe.toggle.call(this,!1,t)},setLanguage:function(e){var t=!(arguments.length>1&&void 0!==arguments[1])||arguments[1];if(o.string(e)){var i=e.toLowerCase();this.captions.language=i;var n=oe.getTracks.call(this),a=oe.findTrack.call(this,[i]);oe.set.call(this,n.indexOf(a),t)}else this.debug.warn("Invalid language argument",e)},getTracks:function(){var e=this,t=arguments.length>0&&void 0!==arguments[0]&&arguments[0];return Array.from((this.media||{}).textTracks||[]).filter(function(i){return!e.isHTML5||t||e.captions.meta.has(i)}).filter(function(e){return["captions","subtitles"].includes(e.kind)})},findTrack:function(e){var t=this,i=arguments.length>1&&void 0!==arguments[1]&&arguments[1],n=oe.getTracks.call(this),a=function(e){return Number((t.captions.meta.get(e)||{}).default)},s=Array.from(n).sort(function(e,t){return a(t)-a(e)}),r=void 0;return e.every(function(e){return!(r=s.find(function(t){return t.language===e}))}),r||(i?s[0]:void 0)},getCurrentTrack:function(){return oe.getTracks.call(this)[this.currentTrack]},getLabel:function(e){var t=e;return!o.track(t)&&D.textTracks&&this.captions.toggled&&(t=oe.getCurrentTrack.call(this)),o.track(t)?o.empty(t.label)?o.empty(t.language)?$("enabled",this.config):e.language.toUpperCase():t.label:$("disabled",this.config)},updateCues:function(e){if(this.supported.ui)if(o.element(this.elements.captions))if(o.nullOrUndefined(e)||Array.isArray(e)){var t=e;if(!t){var i=oe.getCurrentTrack.call(this);t=Array.from((i||{}).activeCues||[]).map(function(e){return e.getCueAsHTML()}).map(J)}var n=t.map(function(e){return e.trim()}).join("\n");if(n!==this.elements.captions.innerHTML){C(this.elements.captions);var a=w("span",S(this.config.selectors.caption));a.innerHTML=n,this.elements.captions.appendChild(a),m.call(this,this.media,"cuechange")}}else this.debug.warn("updateCues: Invalid input",e);else this.debug.warn("No captions element to render to")}},ce={enabled:!0,title:"",debug:!1,autoplay:!1,autopause:!0,playsinline:!0,seekTime:10,volume:1,muted:!1,duration:null,displayDuration:!0,invertTime:!0,toggleInvert:!0,ratio:"16:9",clickToPlay:!0,hideControls:!0,resetOnEnd:!1,disableContextMenu:!0,loadSprite:!0,iconPrefix:"plyr",iconUrl:"https://cdn.plyr.io/3.3.12/plyr.svg",blankVideo:"https://cdn.plyr.io/static/blank.mp4",quality:{default:576,options:[4320,2880,2160,1440,1080,720,576,480,360,240,"default"]},loop:{active:!1},speed:{selected:1,options:[.5,.75,1,1.25,1.5,1.75,2]},keyboard:{focused:!0,global:!1},tooltips:{controls:!1,seek:!0},captions:{active:!1,language:"auto",update:!1},fullscreen:{enabled:!0,fallback:!0,iosNative:!1},storage:{enabled:!0,key:"plyr"},controls:["play-large","play","progress","current-time","mute","volume","captions","settings","pip","airplay","fullscreen"],settings:["captions","quality","speed"],i18n:{restart:"Restart",rewind:"Rewind {seektime}s",play:"Play",pause:"Pause",fastForward:"Forward {seektime}s",seek:"Seek",seekLabel:"{currentTime} of {duration}",played:"Played",buffered:"Buffered",currentTime:"Current time",duration:"Duration",volume:"Volume",mute:"Mute",unmute:"Unmute",enableCaptions:"Enable captions",disableCaptions:"Disable captions",enterFullscreen:"Enter fullscreen",exitFullscreen:"Exit fullscreen",frameTitle:"Player for {title}",captions:"Captions",settings:"Settings",menuBack:"Go back to previous menu",speed:"Speed",normal:"Normal",quality:"Quality",loop:"Loop",start:"Start",end:"End",all:"All",reset:"Reset",disabled:"Disabled",enabled:"Enabled",advertisement:"Ad",qualityBadge:{2160:"4K",1440:"HD",1080:"HD",720:"HD",576:"SD",480:"SD"}},urls:{vimeo:{sdk:"https://player.vimeo.com/api/player.js",iframe:"https://player.vimeo.com/video/{0}?{1}",api:"https://vimeo.com/api/v2/video/{0}.json"},youtube:{sdk:"https://www.youtube.com/iframe_api",api:"https://www.googleapis.com/youtube/v3/videos?id={0}&key={1}&fields=items(snippet(title))&part=snippet"},googleIMA:{sdk:"https://imasdk.googleapis.com/js/sdkloader/ima3.js"}},listeners:{seek:null,play:null,pause:null,restart:null,rewind:null,fastForward:null,mute:null,volume:null,captions:null,fullscreen:null,pip:null,airplay:null,speed:null,quality:null,loop:null,language:null},events:["ended","progress","stalled","playing","waiting","canplay","canplaythrough","loadstart","loadeddata","loadedmetadata","timeupdate","volumechange","play","pause","error","seeking","seeked","emptied","ratechange","cuechange","enterfullscreen","exitfullscreen","captionsenabled","captionsdisabled","languagechange","controlshidden","controlsshown","ready","statechange","qualitychange","qualityrequested","adsloaded","adscontentpause","adscontentresume","adstarted","adsmidpoint","adscomplete","adsallcomplete","adsimpression","adsclick"],selectors:{editable:"input, textarea, select, [contenteditable]",container:".plyr",controls:{container:null,wrapper:".plyr__controls"},labels:"[data-plyr]",buttons:{play:'[data-plyr="play"]',pause:'[data-plyr="pause"]',restart:'[data-plyr="restart"]',rewind:'[data-plyr="rewind"]',fastForward:'[data-plyr="fast-forward"]',mute:'[data-plyr="mute"]',captions:'[data-plyr="captions"]',fullscreen:'[data-plyr="fullscreen"]',pip:'[data-plyr="pip"]',airplay:'[data-plyr="airplay"]',settings:'[data-plyr="settings"]',loop:'[data-plyr="loop"]'},inputs:{seek:'[data-plyr="seek"]',volume:'[data-plyr="volume"]',speed:'[data-plyr="speed"]',language:'[data-plyr="language"]',quality:'[data-plyr="quality"]'},display:{currentTime:".plyr__time--current",duration:".plyr__time--duration",buffer:".plyr__progress__buffer",loop:".plyr__progress__loop",volume:".plyr__volume--display"},progress:".plyr__progress",captions:".plyr__captions",caption:".plyr__caption",menu:{quality:".js-plyr__menu__list--quality"}},classNames:{type:"plyr--{0}",provider:"plyr--{0}",video:"plyr__video-wrapper",embed:"plyr__video-embed",embedContainer:"plyr__video-embed__container",poster:"plyr__poster",posterEnabled:"plyr__poster-enabled",ads:"plyr__ads",control:"plyr__control",controlPressed:"plyr__control--pressed",playing:"plyr--playing",paused:"plyr--paused",stopped:"plyr--stopped",loading:"plyr--loading",hover:"plyr--hover",tooltip:"plyr__tooltip",cues:"plyr__cues",hidden:"plyr__sr-only",hideControls:"plyr--hide-controls",isIos:"plyr--is-ios",isTouch:"plyr--is-touch",uiSupported:"plyr--full-ui",noTransition:"plyr--no-transition",menu:{value:"plyr__menu__value",badge:"plyr__badge",open:"plyr--menu-open"},captions:{enabled:"plyr--captions-enabled",active:"plyr--captions-active"},fullscreen:{enabled:"plyr--fullscreen-enabled",fallback:"plyr--fullscreen-fallback"},pip:{supported:"plyr--pip-supported",active:"plyr--pip-active"},airplay:{supported:"plyr--airplay-supported",active:"plyr--airplay-active"},tabFocus:"plyr__tab-focus"},attributes:{embed:{provider:"data-plyr-provider",id:"data-plyr-embed-id"}},keys:{google:null},ads:{enabled:!1,publisherId:""}},ue={html5:"html5",youtube:"youtube",vimeo:"vimeo"},de={audio:"audio",video:"video"};var pe=function(){},he=function(){function e(){var t=arguments.length>0&&void 0!==arguments[0]&&arguments[0];f(this,e),this.enabled=window.console&&t,this.enabled&&this.log("Debugging enabled")}return g(e,[{key:"log",get:function(){return this.enabled?Function.prototype.bind.call(console.log,console):pe}},{key:"warn",get:function(){return this.enabled?Function.prototype.bind.call(console.warn,console):pe}},{key:"error",get:function(){return this.enabled?Function.prototype.bind.call(console.error,console):pe}}]),e}();function me(){if(this.enabled){var e=this.player.elements.buttons.fullscreen;o.element(e)&&(e.pressed=this.active),m.call(this.player,this.target,this.active?"enterfullscreen":"exitfullscreen",!0),V.isIos||function(){var e=arguments.length>0&&void 0!==arguments[0]?arguments[0]:null,t=arguments.length>1&&void 0!==arguments[1]&&arguments[1];if(o.element(e)){var i=x.call(this,"button:not(:disabled), input:not(:disabled), [tabindex]"),n=i[0],a=i[i.length-1];u.call(this,this.elements.container,"keydown",function(e){if("Tab"===e.key&&9===e.keyCode){var t=q();t!==a||e.shiftKey?t===n&&e.shiftKey&&(a.focus(),e.preventDefault()):(n.focus(),e.preventDefault())}},t,!1)}}.call(this.player,this.target,this.active)}}function fe(){var e=arguments.length>0&&void 0!==arguments[0]&&arguments[0];e?this.scrollPosition={x:window.scrollX||0,y:window.scrollY||0}:window.scrollTo(this.scrollPosition.x,this.scrollPosition.y),document.body.style.overflow=e?"hidden":"",N(this.target,this.player.config.classNames.fullscreen.fallback,e),me.call(this)}var ge=function(){function e(t){var i=this;f(this,e),this.player=t,this.prefix=e.prefix,this.property=e.property,this.scrollPosition={x:0,y:0},d.call(this.player,document,"ms"===this.prefix?"MSFullscreenChange":this.prefix+"fullscreenchange",function(){me.call(i)}),d.call(this.player,this.player.elements.container,"dblclick",function(e){o.element(i.player.elements.controls)&&i.player.elements.controls.contains(e.target)||i.toggle()}),this.update()}return g(e,[{key:"update",value:function(){this.enabled?this.player.debug.log((e.native?"Native":"Fallback")+" fullscreen enabled"):this.player.debug.log("Fullscreen not supported and fallback disabled"),N(this.player.elements.container,this.player.config.classNames.fullscreen.enabled,this.enabled)}},{key:"enter",value:function(){this.enabled&&(V.isIos&&this.player.config.fullscreen.iosNative?this.player.playing&&this.target.webkitEnterFullscreen():e.native?this.prefix?o.empty(this.prefix)||this.target[this.prefix+"Request"+this.property]():this.target.requestFullscreen():fe.call(this,!0))}},{key:"exit",value:function(){if(this.enabled)if(V.isIos&&this.player.config.fullscreen.iosNative)this.target.webkitExitFullscreen(),this.player.play();else if(e.native)if(this.prefix){if(!o.empty(this.prefix)){var t="moz"===this.prefix?"Cancel":"Exit";document[""+this.prefix+t+this.property]()}}else(document.cancelFullScreen||document.exitFullscreen).call(document);else fe.call(this,!1)}},{key:"toggle",value:function(){this.active?this.exit():this.enter()}},{key:"enabled",get:function(){return(e.native||this.player.config.fullscreen.fallback)&&this.player.config.fullscreen.enabled&&this.player.supported.ui&&this.player.isVideo}},{key:"active",get:function(){return!!this.enabled&&(e.native?(this.prefix?document[""+this.prefix+this.property+"Element"]:document.fullscreenElement)===this.target:L(this.target,this.player.config.classNames.fullscreen.fallback))}},{key:"target",get:function(){return V.isIos&&this.player.config.fullscreen.iosNative?this.player.media:this.player.elements.container}}],[{key:"native",get:function(){return!!(document.fullscreenEnabled||document.webkitFullscreenEnabled||document.mozFullScreenEnabled||document.msFullscreenEnabled)}},{key:"prefix",get:function(){if(o.function(document.exitFullscreen))return"";var e="";return["webkit","moz","ms"].some(function(t){return!(!o.function(document[t+"ExitFullscreen"])&&!o.function(document[t+"CancelFullScreen"]))&&(e=t,!0)}),e}},{key:"property",get:function(){return"moz"===this.prefix?"FullScreen":"Fullscreen"}}]),e}();function ye(e){var t=arguments.length>1&&void 0!==arguments[1]?arguments[1]:1;return new Promise(function(i,n){var a=new Image,s=function(){delete a.onload,delete a.onerror,(a.naturalWidth>=t?i:n)(a)};Object.assign(a,{onload:s,onerror:s,src:e})})}var ve={addStyleHook:function(){N(this.elements.container,this.config.selectors.container.replace(".",""),!0),N(this.elements.container,this.config.classNames.uiSupported,this.supported.ui)},toggleNativeControls:function(){arguments.length>0&&void 0!==arguments[0]&&arguments[0]&&this.isHTML5?this.media.setAttribute("controls",""):this.media.removeAttribute("controls")},build:function(){var e=this;if(this.listeners.media(),!this.supported.ui)return this.debug.warn("Basic support only for "+this.provider+" "+this.type),void ve.toggleNativeControls.call(this,!0);o.element(this.elements.controls)||(se.inject.call(this),this.listeners.controls()),ve.toggleNativeControls.call(this),this.isHTML5&&oe.setup.call(this),this.volume=null,this.muted=null,this.speed=null,this.loop=null,this.quality=null,se.updateVolume.call(this),se.timeUpdate.call(this),ve.checkPlaying.call(this),N(this.elements.container,this.config.classNames.pip.supported,D.pip&&this.isHTML5&&this.isVideo),N(this.elements.container,this.config.classNames.airplay.supported,D.airplay&&this.isHTML5),N(this.elements.container,this.config.classNames.isIos,V.isIos),N(this.elements.container,this.config.classNames.isTouch,this.touch),this.ready=!0,setTimeout(function(){m.call(e,e.media,"ready")},0),ve.setTitle.call(this),this.poster&&ve.setPoster.call(this,this.poster,!1).catch(function(){}),this.config.duration&&se.durationUpdate.call(this)},setTitle:function(){var e=$("play",this.config);if(o.string(this.config.title)&&!o.empty(this.config.title)&&(e+=", "+this.config.title),Array.from(this.elements.buttons.play||[]).forEach(function(t){t.setAttribute("aria-label",e)}),this.isEmbed){var t=_.call(this,"iframe");if(!o.element(t))return;var i=o.empty(this.config.title)?"video":this.config.title,n=$("frameTitle",this.config);t.setAttribute("title",n.replace("{title}",i))}},togglePoster:function(e){N(this.elements.container,this.config.classNames.posterEnabled,e)},setPoster:function(e){var t=this;return arguments.length>1&&void 0!==arguments[1]&&!arguments[1]||!this.poster?(this.media.setAttribute("poster",e),function(){var e=this;return new Promise(function(t){return e.ready?setTimeout(t,0):d.call(e,e.elements.container,"ready",t)}).then(function(){})}.call(this).then(function(){return ye(e)}).catch(function(i){throw e===t.poster&&ve.togglePoster.call(t,!1),i}).then(function(){if(e!==t.poster)throw new Error("setPoster cancelled by later call to setPoster")}).then(function(){return Object.assign(t.elements.poster.style,{backgroundImage:"url('"+e+"')",backgroundSize:""}),ve.togglePoster.call(t,!0),e})):Promise.reject(new Error("Poster already set"))},checkPlaying:function(e){var t=this;N(this.elements.container,this.config.classNames.playing,this.playing),N(this.elements.container,this.config.classNames.paused,this.paused),N(this.elements.container,this.config.classNames.stopped,this.stopped),Array.from(this.elements.buttons.play||[]).forEach(function(e){e.pressed=t.playing}),o.event(e)&&"timeupdate"===e.type||ve.toggleControls.call(this)},checkLoading:function(e){var t=this;this.loading=["stalled","waiting"].includes(e.type),clearTimeout(this.timers.loading),this.timers.loading=setTimeout(function(){N(t.elements.container,t.config.classNames.loading,t.loading),ve.toggleControls.call(t)},this.loading?250:0)},toggleControls:function(e){var t=this.elements.controls;t&&this.config.hideControls&&this.toggleControls(Boolean(e||this.loading||this.paused||t.pressed||t.hover))}},be=function(){function e(t){f(this,e),this.player=t,this.lastKey=null,this.handleKey=this.handleKey.bind(this),this.toggleMenu=this.toggleMenu.bind(this),this.firstTouch=this.firstTouch.bind(this)}return g(e,[{key:"handleKey",value:function(e){var t=this,i=e.keyCode?e.keyCode:e.which,n="keydown"===e.type,a=n&&i===this.lastKey;if(!(e.altKey||e.ctrlKey||e.metaKey||e.shiftKey)&&o.number(i)){if(n){var s=q();if(o.element(s)&&s!==this.player.elements.inputs.seek&&M(s,this.player.config.selectors.editable))return;switch([32,37,38,39,40,48,49,50,51,52,53,54,56,57,67,70,73,75,76,77,79].includes(i)&&(e.preventDefault(),e.stopPropagation()),i){case 48:case 49:case 50:case 51:case 52:case 53:case 54:case 55:case 56:case 57:a||(t.player.currentTime=t.player.duration/10*(i-48));break;case 32:case 75:a||this.player.togglePlay();break;case 38:this.player.increaseVolume(.1);break;case 40:this.player.decreaseVolume(.1);break;case 77:a||(this.player.muted=!this.player.muted);break;case 39:this.player.forward();break;case 37:this.player.rewind();break;case 70:this.player.fullscreen.toggle();break;case 67:a||this.player.toggleCaptions();break;case 76:this.player.loop=!this.player.loop}!this.player.fullscreen.enabled&&this.player.fullscreen.active&&27===i&&this.player.fullscreen.toggle(),this.lastKey=i}else this.lastKey=null}}},{key:"toggleMenu",value:function(e){se.toggleMenu.call(this.player,e)}},{key:"firstTouch",value:function(){this.player.touch=!0,N(this.player.elements.container,this.player.config.classNames.isTouch,!0)}},{key:"global",value:function(){var e=!(arguments.length>0&&void 0!==arguments[0])||arguments[0];this.player.config.keyboard.global&&u.call(this.player,window,"keydown keyup",this.handleKey,e,!1),u.call(this.player,document.body,"click",this.toggleMenu,e),h.call(this.player,document.body,"touchstart",this.firstTouch)}},{key:"container",value:function(){var e=this;!this.player.config.keyboard.global&&this.player.config.keyboard.focused&&d.call(this.player,this.player.elements.container,"keydown keyup",this.handleKey,!1),d.call(this.player,this.player.elements.container,"focusout",function(t){N(t.target,e.player.config.classNames.tabFocus,!1)}),d.call(this.player,this.player.elements.container,"keydown",function(t){9===t.keyCode&&setTimeout(function(){N(q(),e.player.config.classNames.tabFocus,!0)},0)}),d.call(this.player,this.player.elements.container,"mousemove mouseleave touchstart touchmove enterfullscreen exitfullscreen",function(t){var i=e.player.elements.controls;"enterfullscreen"===t.type&&(i.pressed=!1,i.hover=!1);var n=0;["touchstart","touchmove","mousemove"].includes(t.type)&&(ve.toggleControls.call(e.player,!0),n=e.player.touch?3e3:2e3),clearTimeout(e.player.timers.controls),e.player.timers.controls=setTimeout(function(){return ve.toggleControls.call(e.player,!1)},n)})}},{key:"media",value:function(){var e=this;if(d.call(this.player,this.player.media,"timeupdate seeking seeked",function(t){return se.timeUpdate.call(e.player,t)}),d.call(this.player,this.player.media,"durationchange loadeddata loadedmetadata",function(t){return se.durationUpdate.call(e.player,t)}),d.call(this.player,this.player.media,"canplay",function(){P(e.player.elements.volume,!e.player.hasAudio),P(e.player.elements.buttons.mute,!e.player.hasAudio)}),d.call(this.player,this.player.media,"ended",function(){e.player.isHTML5&&e.player.isVideo&&e.player.config.resetOnEnd&&e.player.restart()}),d.call(this.player,this.player.media,"progress playing seeking seeked",function(t){return se.updateProgress.call(e.player,t)}),d.call(this.player,this.player.media,"volumechange",function(t){return se.updateVolume.call(e.player,t)}),d.call(this.player,this.player.media,"playing play pause ended emptied timeupdate",function(t){return ve.checkPlaying.call(e.player,t)}),d.call(this.player,this.player.media,"waiting canplay seeked playing",function(t){return ve.checkLoading.call(e.player,t)}),d.call(this.player,this.player.media,"playing",function(){e.player.ads&&e.player.ads.enabled&&!e.player.ads.initialized&&e.player.ads.managerPromise.then(function(){return e.player.ads.play()}).catch(function(){return e.player.play()})}),this.player.supported.ui&&this.player.config.clickToPlay&&!this.player.isAudio){var t=_.call(this.player,"."+this.player.config.classNames.video);if(!o.element(t))return;d.call(this.player,t,"click",function(){e.player.config.hideControls&&e.player.touch&&!e.player.paused||(e.player.paused?e.player.play():e.player.ended?(e.player.restart(),e.player.play()):e.player.pause())})}this.player.supported.ui&&this.player.config.disableContextMenu&&d.call(this.player,this.player.elements.wrapper,"contextmenu",function(e){e.preventDefault()},!1),d.call(this.player,this.player.media,"volumechange",function(){e.player.storage.set({volume:e.player.volume,muted:e.player.muted})}),d.call(this.player,this.player.media,"ratechange",function(){se.updateSetting.call(e.player,"speed"),e.player.storage.set({speed:e.player.speed})}),d.call(this.player,this.player.media,"qualityrequested",function(t){e.player.storage.set({quality:t.detail.quality})}),d.call(this.player,this.player.media,"qualitychange",function(t){se.updateSetting.call(e.player,"quality",null,t.detail.quality)});var i=this.player.config.events.concat(["keyup","keydown"]).join(" ");d.call(this.player,this.player.media,i,function(t){var i=t.detail,n=void 0===i?{}:i;"error"===t.type&&(n=e.player.media.error),m.call(e.player,e.player.elements.container,t.type,!0,n)})}},{key:"controls",value:function(){var e=this,t=V.isIE?"change":"input",i=function(t,i,n){var a=e.player.config.listeners[n],s=!0;o.function(a)&&(s=a.call(e.player,t)),s&&o.function(i)&&i.call(e.player,t)},n=function(t,n,a,s){var r=!(arguments.length>4&&void 0!==arguments[4])||arguments[4],l=e.player.config.listeners[s],c=o.function(l);d.call(e.player,t,n,function(e){return i(e,a,s)},r&&!c)};this.player.elements.buttons.play&&Array.from(this.player.elements.buttons.play).forEach(function(t){n(t,"click",e.player.togglePlay,"play")}),n(this.player.elements.buttons.restart,"click",this.player.restart,"restart"),n(this.player.elements.buttons.rewind,"click",this.player.rewind,"rewind"),n(this.player.elements.buttons.fastForward,"click",this.player.forward,"fastForward"),n(this.player.elements.buttons.mute,"click",function(){e.player.muted=!e.player.muted},"mute"),n(this.player.elements.buttons.captions,"click",function(){return e.player.toggleCaptions()}),n(this.player.elements.buttons.fullscreen,"click",function(){e.player.fullscreen.toggle()},"fullscreen"),n(this.player.elements.buttons.pip,"click",function(){e.player.pip="toggle"},"pip"),n(this.player.elements.buttons.airplay,"click",this.player.airplay,"airplay"),n(this.player.elements.buttons.settings,"click",function(t){se.toggleMenu.call(e.player,t)}),n(this.player.elements.settings.form,"click",function(t){t.stopPropagation();var n=function(){var t="plyr-settings-"+e.player.id+"-home";se.showTab.call(e.player,t)};if(M(t.target,e.player.config.selectors.inputs.language))i(t,function(){e.player.currentTrack=Number(t.target.value),n()},"language");else if(M(t.target,e.player.config.selectors.inputs.quality))i(t,function(){e.player.quality=t.target.value,n()},"quality");else if(M(t.target,e.player.config.selectors.inputs.speed))i(t,function(){e.player.speed=parseFloat(t.target.value),n()},"speed");else{var a=t.target;se.showTab.call(e.player,a.getAttribute("aria-controls"))}}),n(this.player.elements.inputs.seek,"mousedown mousemove",function(t){var i=e.player.elements.progress.getBoundingClientRect(),n=100/i.width*(t.pageX-i.left);t.currentTarget.setAttribute("seek-value",n)}),n(this.player.elements.inputs.seek,"mousedown mouseup keydown keyup touchstart touchend",function(t){var i=t.currentTarget,n=t.keyCode?t.keyCode:t.which,a=t.type;if("keydown"!==a&&"keyup"!==a||39===n||37===n){var s=i.hasAttribute("play-on-seeked"),r=["mouseup","touchend","keyup"].includes(t.type);s&&r?(i.removeAttribute("play-on-seeked"),e.player.play()):!r&&e.player.playing&&(i.setAttribute("play-on-seeked",""),e.player.pause())}}),n(this.player.elements.inputs.seek,t,function(t){var i=t.currentTarget,n=i.getAttribute("seek-value");o.empty(n)&&(n=i.value),i.removeAttribute("seek-value"),e.player.currentTime=n/i.max*e.player.duration},"seek"),this.player.config.toggleInvert&&!o.element(this.player.elements.display.duration)&&n(this.player.elements.display.currentTime,"click",function(){0!==e.player.currentTime&&(e.player.config.invertTime=!e.player.config.invertTime,se.timeUpdate.call(e.player))}),n(this.player.elements.inputs.volume,t,function(t){e.player.volume=t.target.value},"volume"),V.isWebkit&&Array.from(x.call(this.player,'input[type="range"]')).forEach(function(t){n(t,"input",function(t){return se.updateRangeFill.call(e.player,t.target)})}),n(this.player.elements.progress,"mouseenter mouseleave mousemove",function(t){return se.updateSeekTooltip.call(e.player,t)}),n(this.player.elements.controls,"mouseenter mouseleave",function(t){e.player.elements.controls.hover=!e.player.touch&&"mouseenter"===t.type}),n(this.player.elements.controls,"mousedown mouseup touchstart touchend touchcancel",function(t){e.player.elements.controls.pressed=["mousedown","touchstart"].includes(t.type)}),n(this.player.elements.controls,"focusin focusout",function(t){var i=e.player,n=i.config,a=i.elements,s=i.timers;if(N(a.controls,n.classNames.noTransition,"focusin"===t.type),ve.toggleControls.call(e.player,"focusin"===t.type),"focusin"===t.type){setTimeout(function(){N(a.controls,n.classNames.noTransition,!1)},0);var r=e.touch?3e3:4e3;clearTimeout(s.controls),s.controls=setTimeout(function(){return ve.toggleControls.call(e.player,!1)},r)}}),n(this.player.elements.inputs.volume,"wheel",function(t){var i=t.webkitDirectionInvertedFromDevice,n=[t.deltaX,-t.deltaY].map(function(e){return i?-e:e}),a=v(n,2),s=a[0],r=a[1],l=Math.sign(Math.abs(s)>Math.abs(r)?s:r);e.player.increaseVolume(l/50);var o=e.player.media.volume;(1===l&&o<1||-1===l&&o>0)&&t.preventDefault()},"volume",!1)}}]),e}();"undefined"!=typeof window?window:"undefined"!=typeof global?global:"undefined"!=typeof self&&self;var ke,we=(function(e,t){var i;i=function(){var e=function(){},t={},i={},n={};function a(e,t){if(e){var a=n[e];if(i[e]=t,a)for(;a.length;)a[0](e,t),a.splice(0,1)}}function s(t,i){t.call&&(t={success:t}),i.length?(t.error||e)(i):(t.success||e)(t)}function r(t,i,n,a){var s,l,o=document,c=n.async,u=(n.numRetries||0)+1,d=n.before||e,p=t.replace(/^(css|img)!/,"");a=a||0,/(^css!|\.css$)/.test(t)?(s=!0,(l=o.createElement("link")).rel="stylesheet",l.href=p):/(^img!|\.(png|gif|jpg|svg)$)/.test(t)?(l=o.createElement("img")).src=p:((l=o.createElement("script")).src=t,l.async=void 0===c||c),l.onload=l.onerror=l.onbeforeload=function(e){var o=e.type[0];if(s&&"hideFocus"in l)try{l.sheet.cssText.length||(o="e")}catch(e){o="e"}if("e"==o&&(a+=1)<u)return r(t,i,n,a);i(t,o,e.defaultPrevented)},!1!==d(t,l)&&o.head.appendChild(l)}function l(e,i,n){var l,o;if(i&&i.trim&&(l=i),o=(l?n:i)||{},l){if(l in t)throw"LoadJS";t[l]=!0}!function(e,t,i){var n,a,s=(e=e.push?e:[e]).length,l=s,o=[];for(n=function(e,i,n){if("e"==i&&o.push(e),"b"==i){if(!n)return;o.push(e)}--s||t(o)},a=0;a<l;a++)r(e[a],n,i)}(e,function(e){s(o,e),a(l,e)},o)}return l.ready=function(e,t){return function(e,t){e=e.push?e:[e];var a,s,r,l=[],o=e.length,c=o;for(a=function(e,i){i.length&&l.push(e),--c||t(l)};o--;)s=e[o],(r=i[s])?a(s,r):(n[s]=n[s]||[]).push(a)}(e,function(e){s(t,e)}),l},l.done=function(e){a(e,[])},l.reset=function(){t={},i={},n={}},l.isDefined=function(e){return e in t},l},e.exports=i()}(ke={exports:{}},ke.exports),ke.exports);function Te(e){return new Promise(function(t,i){we(e,{success:t,error:i})})}function Ae(e){e&&!this.embed.hasPlayed&&(this.embed.hasPlayed=!0),this.media.paused===e&&(this.media.paused=!e,m.call(this,this.media,e?"play":"pause"))}var Ce={setup:function(){var e=this;N(this.elements.wrapper,this.config.classNames.embed,!0),Ce.setAspectRatio.call(this),o.object(window.Vimeo)?Ce.ready.call(this):Te(this.config.urls.vimeo.sdk).then(function(){Ce.ready.call(e)}).catch(function(t){e.debug.warn("Vimeo API failed to load",t)})},setAspectRatio:function(e){var t=(o.string(e)?e:this.config.ratio).split(":"),i=v(t,2),n=100/i[0]*i[1];if(this.elements.wrapper.style.paddingBottom=n+"%",this.supported.ui){var a=(240-n)/4.8;this.media.style.transform="translateY(-"+a+"%)"}},ready:function(){var e=this,t=this,i=le({loop:t.config.loop.active,autoplay:t.autoplay,byline:!1,portrait:!1,title:!1,speed:!0,transparent:0,gesture:"media",playsinline:!this.config.fullscreen.iosNative}),n=t.media.getAttribute("src");o.empty(n)&&(n=t.media.getAttribute(t.config.attributes.embed.id));var a,s=(a=n,o.empty(a)?null:o.number(Number(a))?a:a.match(/^.*(vimeo.com\/|video\/)(\d+).*/)?RegExp.$2:a),r=w("iframe"),l=W(t.config.urls.vimeo.iframe,s,i);r.setAttribute("src",l),r.setAttribute("allowfullscreen",""),r.setAttribute("allowtransparency",""),r.setAttribute("allow","autoplay");var c=w("div",{poster:t.poster,class:t.config.classNames.embedContainer});c.appendChild(r),t.media=E(c,t.media),Z(W(t.config.urls.vimeo.api,s),"json").then(function(e){if(!o.empty(e)){var i=new URL(e[0].thumbnail_large);i.pathname=i.pathname.split("_")[0]+".jpg",ve.setPoster.call(t,i.href).catch(function(){})}}),t.embed=new window.Vimeo.Player(r,{autopause:t.config.autopause,muted:t.muted}),t.media.paused=!0,t.media.currentTime=0,t.supported.ui&&t.embed.disableTextTrack(),t.media.play=function(){return Ae.call(t,!0),t.embed.play()},t.media.pause=function(){return Ae.call(t,!1),t.embed.pause()},t.media.stop=function(){t.pause(),t.currentTime=0};var u=t.media.currentTime;Object.defineProperty(t.media,"currentTime",{get:function(){return u},set:function(e){var i=t.embed,n=t.media,a=t.paused,s=t.volume,r=a&&!i.hasPlayed;n.seeking=!0,m.call(t,n,"seeking"),Promise.resolve(r&&i.setVolume(0)).then(function(){return i.setCurrentTime(e)}).then(function(){return r&&i.pause()}).then(function(){return r&&i.setVolume(s)}).catch(function(){})}});var d=t.config.speed.selected;Object.defineProperty(t.media,"playbackRate",{get:function(){return d},set:function(e){t.embed.setPlaybackRate(e).then(function(){d=e,m.call(t,t.media,"ratechange")}).catch(function(e){"Error"===e.name&&se.setSpeedMenu.call(t,[])})}});var p=t.config.volume;Object.defineProperty(t.media,"volume",{get:function(){return p},set:function(e){t.embed.setVolume(e).then(function(){p=e,m.call(t,t.media,"volumechange")})}});var h=t.config.muted;Object.defineProperty(t.media,"muted",{get:function(){return h},set:function(e){var i=!!o.boolean(e)&&e;t.embed.setVolume(i?0:t.config.volume).then(function(){h=i,m.call(t,t.media,"volumechange")})}});var f=t.config.loop;Object.defineProperty(t.media,"loop",{get:function(){return f},set:function(e){var i=o.boolean(e)?e:t.config.loop.active;t.embed.setLoop(i).then(function(){f=i})}});var g=void 0;t.embed.getVideoUrl().then(function(e){g=e}).catch(function(t){e.debug.warn(t)}),Object.defineProperty(t.media,"currentSrc",{get:function(){return g}}),Object.defineProperty(t.media,"ended",{get:function(){return t.currentTime===t.duration}}),Promise.all([t.embed.getVideoWidth(),t.embed.getVideoHeight()]).then(function(t){var i=function(e,t){var i=function e(t,i){return 0===i?t:e(i,t%i)}(e,t);return e/i+":"+t/i}(t[0],t[1]);Ce.setAspectRatio.call(e,i)}),t.embed.setAutopause(t.config.autopause).then(function(e){t.config.autopause=e}),t.embed.getVideoTitle().then(function(i){t.config.title=i,ve.setTitle.call(e)}),t.embed.getCurrentTime().then(function(e){u=e,m.call(t,t.media,"timeupdate")}),t.embed.getDuration().then(function(e){t.media.duration=e,m.call(t,t.media,"durationchange")}),t.embed.getTextTracks().then(function(e){t.media.textTracks=e,oe.setup.call(t)}),t.embed.on("cuechange",function(e){var i=e.cues,n=(void 0===i?[]:i).map(function(e){return t=e.text,i=document.createDocumentFragment(),n=document.createElement("div"),i.appendChild(n),n.innerHTML=t,i.firstChild.innerText;var t,i,n});oe.updateCues.call(t,n)}),t.embed.on("loaded",function(){(t.embed.getPaused().then(function(e){Ae.call(t,!e),e||m.call(t,t.media,"playing")}),o.element(t.embed.element)&&t.supported.ui)&&t.embed.element.setAttribute("tabindex",-1)}),t.embed.on("play",function(){Ae.call(t,!0),m.call(t,t.media,"playing")}),t.embed.on("pause",function(){Ae.call(t,!1)}),t.embed.on("timeupdate",function(e){t.media.seeking=!1,u=e.seconds,m.call(t,t.media,"timeupdate")}),t.embed.on("progress",function(e){t.media.buffered=e.percent,m.call(t,t.media,"progress"),1===parseInt(e.percent,10)&&m.call(t,t.media,"canplaythrough"),t.embed.getDuration().then(function(e){e!==t.media.duration&&(t.media.duration=e,m.call(t,t.media,"durationchange"))})}),t.embed.on("seeked",function(){t.media.seeking=!1,m.call(t,t.media,"seeked")}),t.embed.on("ended",function(){t.media.paused=!0,m.call(t,t.media,"ended")}),t.embed.on("error",function(e){t.media.error=e,m.call(t,t.media,"error")}),setTimeout(function(){return ve.build.call(t)},0)}};function Ee(e){var t=Object.entries({hd2160:2160,hd1440:1440,hd1080:1080,hd720:720,large:480,medium:360,small:240,tiny:144}).find(function(t){return t.includes(e)});return t?t.find(function(t){return t!==e}):"default"}function Se(e){e&&!this.embed.hasPlayed&&(this.embed.hasPlayed=!0),this.media.paused===e&&(this.media.paused=!e,m.call(this,this.media,e?"play":"pause"))}var Pe,Ne={setup:function(){var e=this;N(this.elements.wrapper,this.config.classNames.embed,!0),Ne.setAspectRatio.call(this),o.object(window.YT)&&o.function(window.YT.Player)?Ne.ready.call(this):(Te(this.config.urls.youtube.sdk).catch(function(t){e.debug.warn("YouTube API failed to load",t)}),window.onYouTubeReadyCallbacks=window.onYouTubeReadyCallbacks||[],window.onYouTubeReadyCallbacks.push(function(){Ne.ready.call(e)}),window.onYouTubeIframeAPIReady=function(){window.onYouTubeReadyCallbacks.forEach(function(e){e()})})},getTitle:function(e){var t=this;if(o.function(this.embed.getVideoData)){var i=this.embed.getVideoData().title;if(o.empty(i))return this.config.title=i,void ve.setTitle.call(this)}var n=this.config.keys.google;o.string(n)&&!o.empty(n)&&Z(W(this.config.urls.youtube.api,e,n)).then(function(e){o.object(e)&&(t.config.title=e.items[0].snippet.title,ve.setTitle.call(t))}).catch(function(){})},setAspectRatio:function(){var e=this.config.ratio.split(":");this.elements.wrapper.style.paddingBottom=100/e[0]*e[1]+"%"},ready:function(){var e=this,t=e.media.getAttribute("id");if(o.empty(t)||!t.startsWith("youtube-")){var i=e.media.getAttribute("src");o.empty(i)&&(i=e.media.getAttribute(this.config.attributes.embed.id));var n,a=(n=i,o.empty(n)?null:n.match(/^.*(youtu.be\/|v\/|u\/\w\/|embed\/|watch\?v=|&v=)([^#&?]*).*/)?RegExp.$2:n),s=e.provider+"-"+Math.floor(1e4*Math.random()),r=w("div",{id:s,poster:e.poster});e.media=E(r,e.media);var l=function(e){return"https://img.youtube.com/vi/"+a+"/"+e+"default.jpg"};ye(l("maxres"),121).catch(function(){return ye(l("sd"),121)}).catch(function(){return ye(l("hq"))}).then(function(t){return ve.setPoster.call(e,t.src)}).then(function(t){t.includes("maxres")||(e.elements.poster.style.backgroundSize="cover")}).catch(function(){}),e.embed=new window.YT.Player(s,{videoId:a,playerVars:{autoplay:e.config.autoplay?1:0,controls:e.supported.ui?0:1,rel:0,showinfo:0,iv_load_policy:3,modestbranding:1,disablekb:1,playsinline:1,widget_referrer:window?window.location.href:null,cc_load_policy:e.captions.active?1:0,cc_lang_pref:e.config.captions.language},events:{onError:function(t){if(!e.media.error){var i=t.data,n={2:"The request contains an invalid parameter value. For example, this error occurs if you specify a video ID that does not have 11 characters, or if the video ID contains invalid characters, such as exclamation points or asterisks.",5:"The requested content cannot be played in an HTML5 player or another error related to the HTML5 player has occurred.",100:"The video requested was not found. This error occurs when a video has been removed (for any reason) or has been marked as private.",101:"The owner of the requested video does not allow it to be played in embedded players.",150:"The owner of the requested video does not allow it to be played in embedded players."}[i]||"An unknown error occured";e.media.error={code:i,message:n},m.call(e,e.media,"error")}},onPlaybackQualityChange:function(){m.call(e,e.media,"qualitychange",!1,{quality:e.media.quality})},onPlaybackRateChange:function(t){var i=t.target;e.media.playbackRate=i.getPlaybackRate(),m.call(e,e.media,"ratechange")},onReady:function(t){var i=t.target;Ne.getTitle.call(e,a),e.media.play=function(){Se.call(e,!0),i.playVideo()},e.media.pause=function(){Se.call(e,!1),i.pauseVideo()},e.media.stop=function(){i.stopVideo()},e.media.duration=i.getDuration(),e.media.paused=!0,e.media.currentTime=0,Object.defineProperty(e.media,"currentTime",{get:function(){return Number(i.getCurrentTime())},set:function(t){e.paused&&!e.embed.hasPlayed&&e.embed.mute(),e.media.seeking=!0,m.call(e,e.media,"seeking"),i.seekTo(t)}}),Object.defineProperty(e.media,"playbackRate",{get:function(){return i.getPlaybackRate()},set:function(e){i.setPlaybackRate(e)}}),Object.defineProperty(e.media,"quality",{get:function(){return Ee(i.getPlaybackQuality())},set:function(e){i.setPlaybackQuality(Ee(e))}});var n=e.config.volume;Object.defineProperty(e.media,"volume",{get:function(){return n},set:function(t){n=t,i.setVolume(100*n),m.call(e,e.media,"volumechange")}});var s=e.config.muted;Object.defineProperty(e.media,"muted",{get:function(){return s},set:function(t){var n=o.boolean(t)?t:s;s=n,i[n?"mute":"unMute"](),m.call(e,e.media,"volumechange")}}),Object.defineProperty(e.media,"currentSrc",{get:function(){return i.getVideoUrl()}}),Object.defineProperty(e.media,"ended",{get:function(){return e.currentTime===e.duration}}),e.options.speed=i.getAvailablePlaybackRates(),e.supported.ui&&e.media.setAttribute("tabindex",-1),m.call(e,e.media,"timeupdate"),m.call(e,e.media,"durationchange"),clearInterval(e.timers.buffering),e.timers.buffering=setInterval(function(){e.media.buffered=i.getVideoLoadedFraction(),(null===e.media.lastBuffered||e.media.lastBuffered<e.media.buffered)&&m.call(e,e.media,"progress"),e.media.lastBuffered=e.media.buffered,1===e.media.buffered&&(clearInterval(e.timers.buffering),m.call(e,e.media,"canplaythrough"))},200),setTimeout(function(){return ve.build.call(e)},50)},onStateChange:function(t){var i,n=t.target;switch(clearInterval(e.timers.playing),e.media.seeking&&[1,2].includes(t.data)&&(e.media.seeking=!1,m.call(e,e.media,"seeked")),t.data){case-1:m.call(e,e.media,"timeupdate"),e.media.buffered=n.getVideoLoadedFraction(),m.call(e,e.media,"progress");break;case 0:Se.call(e,!1),e.media.loop?(n.stopVideo(),n.playVideo()):m.call(e,e.media,"ended");break;case 1:e.media.paused&&!e.embed.hasPlayed?e.media.pause():(Se.call(e,!0),m.call(e,e.media,"playing"),e.timers.playing=setInterval(function(){m.call(e,e.media,"timeupdate")},50),e.media.duration!==n.getDuration()&&(e.media.duration=n.getDuration(),m.call(e,e.media,"durationchange")),se.setQualityMenu.call(e,(i=n.getAvailableQualityLevels(),o.empty(i)?i:G(i.map(function(e){return Ee(e)})))));break;case 2:e.muted||e.embed.unMute(),Se.call(e,!1)}m.call(e,e.elements.container,"statechange",!1,{code:t.data})}}})}}},Le={setup:function(){this.media?(N(this.elements.container,this.config.classNames.type.replace("{0}",this.type),!0),N(this.elements.container,this.config.classNames.provider.replace("{0}",this.provider),!0),this.isEmbed&&N(this.elements.container,this.config.classNames.type.replace("{0}","video"),!0),this.isVideo&&(this.elements.wrapper=w("div",{class:this.config.classNames.video}),b(this.media,this.elements.wrapper),this.elements.poster=w("div",{class:this.config.classNames.poster}),this.elements.wrapper.appendChild(this.elements.poster)),this.isHTML5?F.extend.call(this):this.isYouTube?Ne.setup.call(this):this.isVimeo&&Ce.setup.call(this)):this.debug.warn("No media element found!")}},Me=function(){function e(t){var i=this;f(this,e),this.player=t,this.publisherId=t.config.ads.publisherId,this.playing=!1,this.initialized=!1,this.elements={container:null,displayContainer:null},this.manager=null,this.loader=null,this.cuePoints=null,this.events={},this.safetyTimer=null,this.countdownTimer=null,this.managerPromise=new Promise(function(e,t){i.on("loaded",e),i.on("error",t)}),this.load()}return g(e,[{key:"load",value:function(){var e=this;this.enabled&&(o.object(window.google)&&o.object(window.google.ima)?this.ready():Te(this.player.config.urls.googleIMA.sdk).then(function(){e.ready()}).catch(function(){e.trigger("error",new Error("Google IMA SDK failed to load"))}))}},{key:"ready",value:function(){var e=this;this.startSafetyTimer(12e3,"ready()"),this.managerPromise.then(function(){e.clearSafetyTimer("onAdsManagerLoaded()")}),this.listeners(),this.setupIMA()}},{key:"setupIMA",value:function(){this.elements.container=w("div",{class:this.player.config.classNames.ads}),this.player.elements.container.appendChild(this.elements.container),google.ima.settings.setVpaidMode(google.ima.ImaSdkSettings.VpaidMode.ENABLED),google.ima.settings.setLocale(this.player.config.ads.language),this.elements.displayContainer=new google.ima.AdDisplayContainer(this.elements.container),this.requestAds()}},{key:"requestAds",value:function(){var e=this,t=this.player.elements.container;try{this.loader=new google.ima.AdsLoader(this.elements.displayContainer),this.loader.addEventListener(google.ima.AdsManagerLoadedEvent.Type.ADS_MANAGER_LOADED,function(t){return e.onAdsManagerLoaded(t)},!1),this.loader.addEventListener(google.ima.AdErrorEvent.Type.AD_ERROR,function(t){return e.onAdError(t)},!1);var i=new google.ima.AdsRequest;i.adTagUrl=this.tagUrl,i.linearAdSlotWidth=t.offsetWidth,i.linearAdSlotHeight=t.offsetHeight,i.nonLinearAdSlotWidth=t.offsetWidth,i.nonLinearAdSlotHeight=t.offsetHeight,i.forceNonLinearFullSlot=!1,i.setAdWillPlayMuted(!this.player.muted),this.loader.requestAds(i)}catch(e){this.onAdError(e)}}},{key:"pollCountdown",value:function(){var e=this;if(!(arguments.length>0&&void 0!==arguments[0]&&arguments[0]))return clearInterval(this.countdownTimer),void this.elements.container.removeAttribute("data-badge-text");this.countdownTimer=setInterval(function(){var t=ae(Math.max(e.manager.getRemainingTime(),0)),i=$("advertisement",e.player.config)+" - "+t;e.elements.container.setAttribute("data-badge-text",i)},100)}},{key:"onAdsManagerLoaded",value:function(e){var t=this,i=new google.ima.AdsRenderingSettings;i.restoreCustomPlaybackStateOnAdBreakComplete=!0,i.enablePreloading=!0,this.manager=e.getAdsManager(this.player,i),this.cuePoints=this.manager.getCuePoints(),o.empty(this.cuePoints)||this.cuePoints.forEach(function(e){if(0!==e&&-1!==e&&e<t.player.duration){var i=t.player.elements.progress;if(o.element(i)){var n=100/t.player.duration*e,a=w("span",{class:t.player.config.classNames.cues});a.style.left=n.toString()+"%",i.appendChild(a)}}}),this.manager.setVolume(this.player.volume),this.manager.addEventListener(google.ima.AdErrorEvent.Type.AD_ERROR,function(e){return t.onAdError(e)}),Object.keys(google.ima.AdEvent.Type).forEach(function(e){t.manager.addEventListener(google.ima.AdEvent.Type[e],function(e){return t.onAdEvent(e)})}),this.trigger("loaded")}},{key:"onAdEvent",value:function(e){var t=this,i=this.player.elements.container,n=e.getAd(),a=function(e){var i="ads"+e.replace(/_/g,"").toLowerCase();m.call(t.player,t.player.media,i)};switch(e.type){case google.ima.AdEvent.Type.LOADED:this.trigger("loaded"),a(e.type),this.pollCountdown(!0),n.isLinear()||(n.width=i.offsetWidth,n.height=i.offsetHeight);break;case google.ima.AdEvent.Type.ALL_ADS_COMPLETED:a(e.type),this.loadAds();break;case google.ima.AdEvent.Type.CONTENT_PAUSE_REQUESTED:a(e.type),this.pauseContent();break;case google.ima.AdEvent.Type.CONTENT_RESUME_REQUESTED:a(e.type),this.pollCountdown(),this.resumeContent();break;case google.ima.AdEvent.Type.STARTED:case google.ima.AdEvent.Type.MIDPOINT:case google.ima.AdEvent.Type.COMPLETE:case google.ima.AdEvent.Type.IMPRESSION:case google.ima.AdEvent.Type.CLICK:a(e.type)}}},{key:"onAdError",value:function(e){this.cancel(),this.player.debug.warn("Ads error",e)}},{key:"listeners",value:function(){var e=this,t=this.player.elements.container,i=void 0;this.player.on("ended",function(){e.loader.contentComplete()}),this.player.on("seeking",function(){return i=e.player.currentTime}),this.player.on("seeked",function(){var t=e.player.currentTime;o.empty(e.cuePoints)||e.cuePoints.forEach(function(n,a){i<n&&n<t&&(e.manager.discardAdBreak(),e.cuePoints.splice(a,1))})}),window.addEventListener("resize",function(){e.manager&&e.manager.resize(t.offsetWidth,t.offsetHeight,google.ima.ViewMode.NORMAL)})}},{key:"play",value:function(){var e=this,t=this.player.elements.container;this.managerPromise||this.resumeContent(),this.managerPromise.then(function(){e.elements.displayContainer.initialize();try{e.initialized||(e.manager.init(t.offsetWidth,t.offsetHeight,google.ima.ViewMode.NORMAL),e.manager.start()),e.initialized=!0}catch(t){e.onAdError(t)}}).catch(function(){})}},{key:"resumeContent",value:function(){this.elements.container.style.zIndex="",this.playing=!1,this.player.currentTime<this.player.duration&&this.player.play()}},{key:"pauseContent",value:function(){this.elements.container.style.zIndex=3,this.playing=!0,this.player.pause()}},{key:"cancel",value:function(){this.initialized&&this.resumeContent(),this.trigger("error"),this.loadAds()}},{key:"loadAds",value:function(){var e=this;this.managerPromise.then(function(){e.manager&&e.manager.destroy(),e.managerPromise=new Promise(function(t){e.on("loaded",t),e.player.debug.log(e.manager)}),e.requestAds()}).catch(function(){})}},{key:"trigger",value:function(e){for(var t=this,i=arguments.length,n=Array(i>1?i-1:0),a=1;a<i;a++)n[a-1]=arguments[a];var s=this.events[e];o.array(s)&&s.forEach(function(e){o.function(e)&&e.apply(t,n)})}},{key:"on",value:function(e,t){return o.array(this.events[e])||(this.events[e]=[]),this.events[e].push(t),this}},{key:"startSafetyTimer",value:function(e,t){var i=this;this.player.debug.log("Safety timer invoked from: "+t),this.safetyTimer=setTimeout(function(){i.cancel(),i.clearSafetyTimer("startSafetyTimer()")},e)}},{key:"clearSafetyTimer",value:function(e){o.nullOrUndefined(this.safetyTimer)||(this.player.debug.log("Safety timer cleared from: "+e),clearTimeout(this.safetyTimer),this.safetyTimer=null)}},{key:"enabled",get:function(){return this.player.isHTML5&&this.player.isVideo&&this.player.config.ads.enabled&&!o.empty(this.publisherId)}},{key:"tagUrl",get:function(){return"https://go.aniview.com/api/adserver6/vast/?"+le({AV_PUBLISHERID:"58c25bb0073ef448b1087ad6",AV_CHANNELID:"5a0458dc28a06145e4519d21",AV_URL:window.location.hostname,cb:Date.now(),AV_WIDTH:640,AV_HEIGHT:480,AV_CDIM2:this.publisherId})}}]),e}(),xe={insertElements:function(e,t){var i=this;o.string(t)?T(e,this.media,{src:t}):o.array(t)&&t.forEach(function(t){T(e,i.media,t)})},change:function(e){var t=this;U(e,"sources.length")?(F.cancelRequests.call(this),this.destroy.call(this,function(){t.options.quality=[],A(t.media),t.media=null,o.element(t.elements.container)&&t.elements.container.removeAttribute("class");var i=e.sources,n=e.type,a=v(i,1)[0],s=a.provider,r=void 0===s?ue.html5:s,l=a.src,c="html5"===r?n:"div",u="html5"===r?{}:{src:l};Object.assign(t,{provider:r,type:n,supported:D.check(n,r,t.config.playsinline),media:w(c,u)}),t.elements.container.appendChild(t.media),o.boolean(e.autoplay)&&(t.config.autoplay=e.autoplay),t.isHTML5&&(t.config.crossorigin&&t.media.setAttribute("crossorigin",""),t.config.autoplay&&t.media.setAttribute("autoplay",""),o.empty(e.poster)||(t.poster=e.poster),t.config.loop.active&&t.media.setAttribute("loop",""),t.config.muted&&t.media.setAttribute("muted",""),t.config.playsinline&&t.media.setAttribute("playsinline","")),ve.addStyleHook.call(t),t.isHTML5&&xe.insertElements.call(t,"source",i),t.config.title=e.title,Le.setup.call(t),t.isHTML5&&("tracks"in e&&xe.insertElements.call(t,"track",e.tracks),t.media.load()),(t.isHTML5||t.isEmbed&&!t.supported.ui)&&ve.build.call(t),t.fullscreen.update()},!0)):this.debug.warn("Invalid source format")}},_e=function(){function e(t,i){var n=this;if(f(this,e),this.timers={},this.ready=!1,this.loading=!1,this.failed=!1,this.touch=D.touch,this.media=t,o.string(this.media)&&(this.media=document.querySelectorAll(this.media)),(window.jQuery&&this.media instanceof jQuery||o.nodeList(this.media)||o.array(this.media))&&(this.media=this.media[0]),this.config=z({},ce,e.defaults,i||{},function(){try{return JSON.parse(n.media.getAttribute("data-plyr-config"))}catch(e){return{}}}()),this.elements={container:null,buttons:{},display:{},progress:{},inputs:{},settings:{menu:null,panes:{},tabs:{}},captions:null},this.captions={active:null,currentTrack:-1,meta:new WeakMap},this.fullscreen={active:!1},this.options={speed:[],quality:[]},this.debug=new he(this.config.debug),this.debug.log("Config",this.config),this.debug.log("Support",D),!o.nullOrUndefined(this.media)&&o.element(this.media))if(this.media.plyr)this.debug.warn("Target already setup");else if(this.config.enabled)if(D.check().api){var a=this.media.cloneNode(!0);a.autoplay=!1,this.elements.original=a;var s=this.media.tagName.toLowerCase(),r=null,l=null;switch(s){case"div":if(r=this.media.querySelector("iframe"),o.element(r)){if(l=re(r.getAttribute("src")),this.provider=function(e){return/^(https?:\/\/)?(www\.)?(youtube\.com|youtu\.?be)\/.+$/.test(e)?ue.youtube:/^https?:\/\/player.vimeo.com\/video\/\d{0,9}(?=\b|\/)/.test(e)?ue.vimeo:null}(l.toString()),this.elements.container=this.media,this.media=r,this.elements.container.className="",l.searchParams.length){var c=["1","true"];c.includes(l.searchParams.get("autoplay"))&&(this.config.autoplay=!0),c.includes(l.searchParams.get("loop"))&&(this.config.loop.active=!0),this.isYouTube?this.config.playsinline=c.includes(l.searchParams.get("playsinline")):this.config.playsinline=!0}}else this.provider=this.media.getAttribute(this.config.attributes.embed.provider),this.media.removeAttribute(this.config.attributes.embed.provider);if(o.empty(this.provider)||!Object.keys(ue).includes(this.provider))return void this.debug.error("Setup failed: Invalid provider");this.type=de.video;break;case"video":case"audio":this.type=s,this.provider=ue.html5,this.media.hasAttribute("crossorigin")&&(this.config.crossorigin=!0),this.media.hasAttribute("autoplay")&&(this.config.autoplay=!0),this.media.hasAttribute("playsinline")&&(this.config.playsinline=!0),this.media.hasAttribute("muted")&&(this.config.muted=!0),this.media.hasAttribute("loop")&&(this.config.loop.active=!0);break;default:return void this.debug.error("Setup failed: unsupported type")}this.supported=D.check(this.type,this.provider,this.config.playsinline),this.supported.api?(this.eventListeners=[],this.listeners=new be(this),this.storage=new X(this),this.media.plyr=this,o.element(this.elements.container)||(this.elements.container=w("div"),b(this.media,this.elements.container)),ve.addStyleHook.call(this),Le.setup.call(this),this.config.debug&&d.call(this,this.elements.container,this.config.events.join(" "),function(e){n.debug.log("event: "+e.type)}),(this.isHTML5||this.isEmbed&&!this.supported.ui)&&ve.build.call(this),this.listeners.container(),this.listeners.global(),this.fullscreen=new ge(this),this.ads=new Me(this),this.config.autoplay&&this.play()):this.debug.error("Setup failed: no support")}else this.debug.error("Setup failed: no support");else this.debug.error("Setup failed: disabled by config");else this.debug.error("Setup failed: no suitable element passed")}return g(e,[{key:"play",value:function(){return o.function(this.media.play)?this.media.play():null}},{key:"pause",value:function(){this.playing&&o.function(this.media.pause)&&this.media.pause()}},{key:"togglePlay",value:function(e){(o.boolean(e)?e:!this.playing)?this.play():this.pause()}},{key:"stop",value:function(){this.isHTML5?(this.pause(),this.restart()):o.function(this.media.stop)&&this.media.stop()}},{key:"restart",value:function(){this.currentTime=0}},{key:"rewind",value:function(e){this.currentTime=this.currentTime-(o.number(e)?e:this.config.seekTime)}},{key:"forward",value:function(e){this.currentTime=this.currentTime+(o.number(e)?e:this.config.seekTime)}},{key:"increaseVolume",value:function(e){var t=this.media.muted?0:this.volume;this.volume=t+(o.number(e)?e:0)}},{key:"decreaseVolume",value:function(e){this.increaseVolume(-e)}},{key:"toggleCaptions",value:function(e){oe.toggle.call(this,e,!1)}},{key:"airplay",value:function(){D.airplay&&this.media.webkitShowPlaybackTargetPicker()}},{key:"toggleControls",value:function(e){if(this.supported.ui&&!this.isAudio){var t=L(this.elements.container,this.config.classNames.hideControls),i=void 0===e?void 0:!e,n=N(this.elements.container,this.config.classNames.hideControls,i);if(n&&this.config.controls.includes("settings")&&!o.empty(this.config.settings)&&se.toggleMenu.call(this,!1),n!==t){var a=n?"controlshidden":"controlsshown";m.call(this,this.media,a)}return!n}return!1}},{key:"on",value:function(e,t){d.call(this,this.elements.container,e,t)}},{key:"once",value:function(e,t){h.call(this,this.elements.container,e,t)}},{key:"off",value:function(e,t){p(this.elements.container,e,t)}},{key:"destroy",value:function(e){var t=this,i=arguments.length>1&&void 0!==arguments[1]&&arguments[1];if(this.ready){var n=function(){document.body.style.overflow="",t.embed=null,i?(Object.keys(t.elements).length&&(A(t.elements.buttons.play),A(t.elements.captions),A(t.elements.controls),A(t.elements.wrapper),t.elements.buttons.play=null,t.elements.captions=null,t.elements.controls=null,t.elements.wrapper=null),o.function(e)&&e()):(function(){this&&this.eventListeners&&(this.eventListeners.forEach(function(e){var t=e.element,i=e.type,n=e.callback,a=e.options;t.removeEventListener(i,n,a)}),this.eventListeners=[])}.call(t),E(t.elements.original,t.elements.container),m.call(t,t.elements.original,"destroyed",!0),o.function(e)&&e.call(t.elements.original),t.ready=!1,setTimeout(function(){t.elements=null,t.media=null},200))};this.stop(),this.isHTML5?(clearTimeout(this.timers.loading),ve.toggleNativeControls.call(this,!0),n()):this.isYouTube?(clearInterval(this.timers.buffering),clearInterval(this.timers.playing),null!==this.embed&&o.function(this.embed.destroy)&&this.embed.destroy(),n()):this.isVimeo&&(null!==this.embed&&this.embed.unload().then(n),setTimeout(n,200))}}},{key:"supports",value:function(e){return D.mime.call(this,e)}},{key:"isHTML5",get:function(){return Boolean(this.provider===ue.html5)}},{key:"isEmbed",get:function(){return Boolean(this.isYouTube||this.isVimeo)}},{key:"isYouTube",get:function(){return Boolean(this.provider===ue.youtube)}},{key:"isVimeo",get:function(){return Boolean(this.provider===ue.vimeo)}},{key:"isVideo",get:function(){return Boolean(this.type===de.video)}},{key:"isAudio",get:function(){return Boolean(this.type===de.audio)}},{key:"playing",get:function(){return Boolean(this.ready&&!this.paused&&!this.ended)}},{key:"paused",get:function(){return Boolean(this.media.paused)}},{key:"stopped",get:function(){return Boolean(this.paused&&0===this.currentTime)}},{key:"ended",get:function(){return Boolean(this.media.ended)}},{key:"currentTime",set:function(e){if(this.duration){var t=o.number(e)&&e>0;this.media.currentTime=t?Math.min(e,this.duration):0,this.debug.log("Seeking to "+this.currentTime+" seconds")}},get:function(){return Number(this.media.currentTime)}},{key:"buffered",get:function(){var e=this.media.buffered;return o.number(e)?e:e&&e.length&&this.duration>0?e.end(0)/this.duration:0}},{key:"seeking",get:function(){return Boolean(this.media.seeking)}},{key:"duration",get:function(){var e=parseFloat(this.config.duration),t=(this.media||{}).duration,i=o.number(t)&&t!==1/0?t:0;return e||i}},{key:"volume",set:function(e){var t=e;o.string(t)&&(t=Number(t)),o.number(t)||(t=this.storage.get("volume")),o.number(t)||(t=this.config.volume),t>1&&(t=1),t<0&&(t=0),this.config.volume=t,this.media.volume=t,!o.empty(e)&&this.muted&&t>0&&(this.muted=!1)},get:function(){return Number(this.media.volume)}},{key:"muted",set:function(e){var t=e;o.boolean(t)||(t=this.storage.get("muted")),o.boolean(t)||(t=this.config.muted),this.config.muted=t,this.media.muted=t},get:function(){return Boolean(this.media.muted)}},{key:"hasAudio",get:function(){return!this.isHTML5||(!!this.isAudio||(Boolean(this.media.mozHasAudio)||Boolean(this.media.webkitAudioDecodedByteCount)||Boolean(this.media.audioTracks&&this.media.audioTracks.length)))}},{key:"speed",set:function(e){var t=null;o.number(e)&&(t=e),o.number(t)||(t=this.storage.get("speed")),o.number(t)||(t=this.config.speed.selected),t<.1&&(t=.1),t>2&&(t=2),this.config.speed.options.includes(t)?(this.config.speed.selected=t,this.media.playbackRate=t):this.debug.warn("Unsupported speed ("+t+")")},get:function(){return Number(this.media.playbackRate)}},{key:"quality",set:function(e){var t=this.config.quality,i=this.options.quality;if(i.length){var n=[!o.empty(e)&&Number(e),this.storage.get("quality"),t.selected,t.default].find(o.number);if(!i.includes(n)){var a=function(e,t){return o.array(e)&&e.length?e.reduce(function(e,i){return Math.abs(i-t)<Math.abs(e-t)?i:e}):null}(i,n);this.debug.warn("Unsupported quality option: "+n+", using "+a+" instead"),n=a}m.call(this,this.media,"qualityrequested",!1,{quality:n}),t.selected=n,this.media.quality=n}},get:function(){return this.media.quality}},{key:"loop",set:function(e){var t=o.boolean(e)?e:this.config.loop.active;this.config.loop.active=t,this.media.loop=t},get:function(){return Boolean(this.media.loop)}},{key:"source",set:function(e){xe.change.call(this,e)},get:function(){return this.media.currentSrc}},{key:"poster",set:function(e){this.isVideo?ve.setPoster.call(this,e,!1).catch(function(){}):this.debug.warn("Poster can only be set for video")},get:function(){return this.isVideo?this.media.getAttribute("poster"):null}},{key:"autoplay",set:function(e){var t=o.boolean(e)?e:this.config.autoplay;this.config.autoplay=t},get:function(){return Boolean(this.config.autoplay)}},{key:"currentTrack",set:function(e){oe.set.call(this,e,!1)},get:function(){var e=this.captions,t=e.toggled,i=e.currentTrack;return t?i:-1}},{key:"language",set:function(e){oe.setLanguage.call(this,e,!1)},get:function(){return(oe.getCurrentTrack.call(this)||{}).language}},{key:"pip",set:function(e){var t="picture-in-picture",i="inline";if(D.pip){var n=o.boolean(e)?e:this.pip===i;this.media.webkitSetPresentationMode(n?t:i)}},get:function(){return D.pip?this.media.webkitPresentationMode:null}}],[{key:"supported",value:function(e,t,i){return D.check(e,t,i)}},{key:"loadSprite",value:function(e,t){return ee(e,t)}},{key:"setup",value:function(t){var i=arguments.length>1&&void 0!==arguments[1]?arguments[1]:{},n=null;return o.string(t)?n=Array.from(document.querySelectorAll(t)):o.nodeList(t)?n=Array.from(t):o.array(t)&&(n=t.filter(o.element)),o.empty(n)?null:n.map(function(t){return new e(t,i)})}}]),e}();return _e.defaults=(Pe=ce,JSON.parse(JSON.stringify(Pe))),_e});
//# sourceMappingURL=plyr.min.js.map
;/*
     _ _      _       _
 ___| (_) ___| | __  (_)___
/ __| | |/ __| |/ /  | / __|
\__ \ | | (__|   < _ | \__ \
|___/_|_|\___|_|\_(_)/ |___/
                   |__/

 Version: 1.8.0
  Author: Ken Wheeler
 Website: http://kenwheeler.github.io
    Docs: http://kenwheeler.github.io/slick
    Repo: http://github.com/kenwheeler/slick
  Issues: http://github.com/kenwheeler/slick/issues

 */
/* global window, document, define, jQuery, setInterval, clearInterval */
;(function(factory) {
    'use strict';
    if (typeof define === 'function' && define.amd) {
        define(['jquery'], factory);
    } else if (typeof exports !== 'undefined') {
        module.exports = factory(require('jquery'));
    } else {
        factory(jQuery);
    }

}(function($) {
    'use strict';
    var Slick = window.Slick || {};

    Slick = (function() {

        var instanceUid = 0;

        function Slick(element, settings) {

            var _ = this, dataSettings;

            _.defaults = {
                accessibility: true,
                adaptiveHeight: false,
                appendArrows: $(element),
                appendDots: $(element),
                arrows: true,
                asNavFor: null,
                prevArrow: '<button class="slick-prev" aria-label="Previous" type="button">Previous</button>',
                nextArrow: '<button class="slick-next" aria-label="Next" type="button">Next</button>',
                autoplay: false,
                autoplaySpeed: 3000,
                centerMode: false,
                centerPadding: '50px',
                cssEase: 'ease',
                customPaging: function(slider, i) {
                    return $('<button type="button" />').text(i + 1);
                },
                dots: false,
                dotsClass: 'slick-dots',
                draggable: true,
                easing: 'linear',
                edgeFriction: 0.35,
                fade: false,
                focusOnSelect: false,
                focusOnChange: false,
                infinite: true,
                initialSlide: 0,
                lazyLoad: 'ondemand',
                mobileFirst: false,
                pauseOnHover: true,
                pauseOnFocus: true,
                pauseOnDotsHover: false,
                respondTo: 'window',
                responsive: null,
                rows: 1,
                rtl: false,
                slide: '',
                slidesPerRow: 1,
                slidesToShow: 1,
                slidesToScroll: 1,
                speed: 500,
                swipe: true,
                swipeToSlide: false,
                touchMove: true,
                touchThreshold: 5,
                useCSS: true,
                useTransform: true,
                variableWidth: false,
                vertical: false,
                verticalSwiping: false,
                waitForAnimate: true,
                zIndex: 1000
            };

            _.initials = {
                animating: false,
                dragging: false,
                autoPlayTimer: null,
                currentDirection: 0,
                currentLeft: null,
                currentSlide: 0,
                direction: 1,
                $dots: null,
                listWidth: null,
                listHeight: null,
                loadIndex: 0,
                $nextArrow: null,
                $prevArrow: null,
                scrolling: false,
                slideCount: null,
                slideWidth: null,
                $slideTrack: null,
                $slides: null,
                sliding: false,
                slideOffset: 0,
                swipeLeft: null,
                swiping: false,
                $list: null,
                touchObject: {},
                transformsEnabled: false,
                unslicked: false
            };

            $.extend(_, _.initials);

            _.activeBreakpoint = null;
            _.animType = null;
            _.animProp = null;
            _.breakpoints = [];
            _.breakpointSettings = [];
            _.cssTransitions = false;
            _.focussed = false;
            _.interrupted = false;
            _.hidden = 'hidden';
            _.paused = true;
            _.positionProp = null;
            _.respondTo = null;
            _.rowCount = 1;
            _.shouldClick = true;
            _.$slider = $(element);
            _.$slidesCache = null;
            _.transformType = null;
            _.transitionType = null;
            _.visibilityChange = 'visibilitychange';
            _.windowWidth = 0;
            _.windowTimer = null;

            dataSettings = $(element).data('slick') || {};

            _.options = $.extend({}, _.defaults, settings, dataSettings);

            _.currentSlide = _.options.initialSlide;

            _.originalSettings = _.options;

            if (typeof document.mozHidden !== 'undefined') {
                _.hidden = 'mozHidden';
                _.visibilityChange = 'mozvisibilitychange';
            } else if (typeof document.webkitHidden !== 'undefined') {
                _.hidden = 'webkitHidden';
                _.visibilityChange = 'webkitvisibilitychange';
            }

            _.autoPlay = $.proxy(_.autoPlay, _);
            _.autoPlayClear = $.proxy(_.autoPlayClear, _);
            _.autoPlayIterator = $.proxy(_.autoPlayIterator, _);
            _.changeSlide = $.proxy(_.changeSlide, _);
            _.clickHandler = $.proxy(_.clickHandler, _);
            _.selectHandler = $.proxy(_.selectHandler, _);
            _.setPosition = $.proxy(_.setPosition, _);
            _.swipeHandler = $.proxy(_.swipeHandler, _);
            _.dragHandler = $.proxy(_.dragHandler, _);
            _.keyHandler = $.proxy(_.keyHandler, _);

            _.instanceUid = instanceUid++;

            // A simple way to check for HTML strings
            // Strict HTML recognition (must start with <)
            // Extracted from jQuery v1.11 source
            _.htmlExpr = /^(?:\s*(<[\w\W]+>)[^>]*)$/;


            _.registerBreakpoints();
            _.init(true);

        }

        return Slick;

    }());

    Slick.prototype.activateADA = function() {
        var _ = this;

        _.$slideTrack.find('.slick-active').attr({
            'aria-hidden': 'false'
        }).find('a, input, button, select').attr({
            'tabindex': '0'
        });

    };

    Slick.prototype.addSlide = Slick.prototype.slickAdd = function(markup, index, addBefore) {

        var _ = this;

        if (typeof(index) === 'boolean') {
            addBefore = index;
            index = null;
        } else if (index < 0 || (index >= _.slideCount)) {
            return false;
        }

        _.unload();

        if (typeof(index) === 'number') {
            if (index === 0 && _.$slides.length === 0) {
                $(markup).appendTo(_.$slideTrack);
            } else if (addBefore) {
                $(markup).insertBefore(_.$slides.eq(index));
            } else {
                $(markup).insertAfter(_.$slides.eq(index));
            }
        } else {
            if (addBefore === true) {
                $(markup).prependTo(_.$slideTrack);
            } else {
                $(markup).appendTo(_.$slideTrack);
            }
        }

        _.$slides = _.$slideTrack.children(this.options.slide);

        _.$slideTrack.children(this.options.slide).detach();

        _.$slideTrack.append(_.$slides);

        _.$slides.each(function(index, element) {
            $(element).attr('data-slick-index', index);
        });

        _.$slidesCache = _.$slides;

        _.reinit();

    };

    Slick.prototype.animateHeight = function() {
        var _ = this;
        if (_.options.slidesToShow === 1 && _.options.adaptiveHeight === true && _.options.vertical === false) {
            var targetHeight = _.$slides.eq(_.currentSlide).outerHeight(true);
            _.$list.animate({
                height: targetHeight
            }, _.options.speed);
        }
    };

    Slick.prototype.animateSlide = function(targetLeft, callback) {

        var animProps = {},
            _ = this;

        _.animateHeight();

        if (_.options.rtl === true && _.options.vertical === false) {
            targetLeft = -targetLeft;
        }
        if (_.transformsEnabled === false) {
            if (_.options.vertical === false) {
                _.$slideTrack.animate({
                    left: targetLeft
                }, _.options.speed, _.options.easing, callback);
            } else {
                _.$slideTrack.animate({
                    top: targetLeft
                }, _.options.speed, _.options.easing, callback);
            }

        } else {

            if (_.cssTransitions === false) {
                if (_.options.rtl === true) {
                    _.currentLeft = -(_.currentLeft);
                }
                $({
                    animStart: _.currentLeft
                }).animate({
                    animStart: targetLeft
                }, {
                    duration: _.options.speed,
                    easing: _.options.easing,
                    step: function(now) {
                        now = Math.ceil(now);
                        if (_.options.vertical === false) {
                            animProps[_.animType] = 'translate(' +
                                now + 'px, 0px)';
                            _.$slideTrack.css(animProps);
                        } else {
                            animProps[_.animType] = 'translate(0px,' +
                                now + 'px)';
                            _.$slideTrack.css(animProps);
                        }
                    },
                    complete: function() {
                        if (callback) {
                            callback.call();
                        }
                    }
                });

            } else {

                _.applyTransition();
                targetLeft = Math.ceil(targetLeft);

                if (_.options.vertical === false) {
                    animProps[_.animType] = 'translate3d(' + targetLeft + 'px, 0px, 0px)';
                } else {
                    animProps[_.animType] = 'translate3d(0px,' + targetLeft + 'px, 0px)';
                }
                _.$slideTrack.css(animProps);

                if (callback) {
                    setTimeout(function() {

                        _.disableTransition();

                        callback.call();
                    }, _.options.speed);
                }

            }

        }

    };

    Slick.prototype.getNavTarget = function() {

        var _ = this,
            asNavFor = _.options.asNavFor;

        if ( asNavFor && asNavFor !== null ) {
            asNavFor = $(asNavFor).not(_.$slider);
        }

        return asNavFor;

    };

    Slick.prototype.asNavFor = function(index) {

        var _ = this,
            asNavFor = _.getNavTarget();

        if ( asNavFor !== null && typeof asNavFor === 'object' ) {
            asNavFor.each(function() {
                var target = $(this).slick('getSlick');
                if(!target.unslicked) {
                    target.slideHandler(index, true);
                }
            });
        }

    };

    Slick.prototype.applyTransition = function(slide) {

        var _ = this,
            transition = {};

        if (_.options.fade === false) {
            transition[_.transitionType] = _.transformType + ' ' + _.options.speed + 'ms ' + _.options.cssEase;
        } else {
            transition[_.transitionType] = 'opacity ' + _.options.speed + 'ms ' + _.options.cssEase;
        }

        if (_.options.fade === false) {
            _.$slideTrack.css(transition);
        } else {
            _.$slides.eq(slide).css(transition);
        }

    };

    Slick.prototype.autoPlay = function() {

        var _ = this;

        _.autoPlayClear();

        if ( _.slideCount > _.options.slidesToShow ) {
            _.autoPlayTimer = setInterval( _.autoPlayIterator, _.options.autoplaySpeed );
        }

    };

    Slick.prototype.autoPlayClear = function() {

        var _ = this;

        if (_.autoPlayTimer) {
            clearInterval(_.autoPlayTimer);
        }

    };

    Slick.prototype.autoPlayIterator = function() {

        var _ = this,
            slideTo = _.currentSlide + _.options.slidesToScroll;

        if ( !_.paused && !_.interrupted && !_.focussed ) {

            if ( _.options.infinite === false ) {

                if ( _.direction === 1 && ( _.currentSlide + 1 ) === ( _.slideCount - 1 )) {
                    _.direction = 0;
                }

                else if ( _.direction === 0 ) {

                    slideTo = _.currentSlide - _.options.slidesToScroll;

                    if ( _.currentSlide - 1 === 0 ) {
                        _.direction = 1;
                    }

                }

            }

            _.slideHandler( slideTo );

        }

    };

    Slick.prototype.buildArrows = function() {

        var _ = this;

        if (_.options.arrows === true ) {

            _.$prevArrow = $(_.options.prevArrow).addClass('slick-arrow');
            _.$nextArrow = $(_.options.nextArrow).addClass('slick-arrow');

            if( _.slideCount > _.options.slidesToShow ) {

                _.$prevArrow.removeClass('slick-hidden').removeAttr('aria-hidden tabindex');
                _.$nextArrow.removeClass('slick-hidden').removeAttr('aria-hidden tabindex');

                if (_.htmlExpr.test(_.options.prevArrow)) {
                    _.$prevArrow.prependTo(_.options.appendArrows);
                }

                if (_.htmlExpr.test(_.options.nextArrow)) {
                    _.$nextArrow.appendTo(_.options.appendArrows);
                }

                if (_.options.infinite !== true) {
                    _.$prevArrow
                        .addClass('slick-disabled')
                        .attr('aria-disabled', 'true');
                }

            } else {

                _.$prevArrow.add( _.$nextArrow )

                    .addClass('slick-hidden')
                    .attr({
                        'aria-disabled': 'true',
                        'tabindex': '-1'
                    });

            }

        }

    };

    Slick.prototype.buildDots = function() {

        var _ = this,
            i, dot;

        if (_.options.dots === true) {

            _.$slider.addClass('slick-dotted');

            dot = $('<ul />').addClass(_.options.dotsClass);

            for (i = 0; i <= _.getDotCount(); i += 1) {
                dot.append($('<li />').append(_.options.customPaging.call(this, _, i)));
            }

            _.$dots = dot.appendTo(_.options.appendDots);

            _.$dots.find('li').first().addClass('slick-active');

        }

    };

    Slick.prototype.buildOut = function() {

        var _ = this;

        _.$slides =
            _.$slider
                .children( _.options.slide + ':not(.slick-cloned)')
                .addClass('slick-slide');

        _.slideCount = _.$slides.length;

        _.$slides.each(function(index, element) {
            $(element)
                .attr('data-slick-index', index)
                .data('originalStyling', $(element).attr('style') || '');
        });

        _.$slider.addClass('slick-slider');

        _.$slideTrack = (_.slideCount === 0) ?
            $('<div class="slick-track"/>').appendTo(_.$slider) :
            _.$slides.wrapAll('<div class="slick-track"/>').parent();

        _.$list = _.$slideTrack.wrap(
            '<div class="slick-list"/>').parent();
        _.$slideTrack.css('opacity', 0);

        if (_.options.centerMode === true || _.options.swipeToSlide === true) {
            _.options.slidesToScroll = 1;
        }

        $('img[data-lazy]', _.$slider).not('[src]').addClass('slick-loading');

        _.setupInfinite();

        _.buildArrows();

        _.buildDots();

        _.updateDots();


        _.setSlideClasses(typeof _.currentSlide === 'number' ? _.currentSlide : 0);

        if (_.options.draggable === true) {
            _.$list.addClass('draggable');
        }

    };

    Slick.prototype.buildRows = function() {

        var _ = this, a, b, c, newSlides, numOfSlides, originalSlides,slidesPerSection;

        newSlides = document.createDocumentFragment();
        originalSlides = _.$slider.children();

        if(_.options.rows > 1) {

            slidesPerSection = _.options.slidesPerRow * _.options.rows;
            numOfSlides = Math.ceil(
                originalSlides.length / slidesPerSection
            );

            for(a = 0; a < numOfSlides; a++){
                var slide = document.createElement('div');
                for(b = 0; b < _.options.rows; b++) {
                    var row = document.createElement('div');
                    for(c = 0; c < _.options.slidesPerRow; c++) {
                        var target = (a * slidesPerSection + ((b * _.options.slidesPerRow) + c));
                        if (originalSlides.get(target)) {
                            row.appendChild(originalSlides.get(target));
                        }
                    }
                    slide.appendChild(row);
                }
                newSlides.appendChild(slide);
            }

            _.$slider.empty().append(newSlides);
            _.$slider.children().children().children()
                .css({
                    'width':(100 / _.options.slidesPerRow) + '%',
                    'display': 'inline-block'
                });

        }

    };

    Slick.prototype.checkResponsive = function(initial, forceUpdate) {

        var _ = this,
            breakpoint, targetBreakpoint, respondToWidth, triggerBreakpoint = false;
        var sliderWidth = _.$slider.width();
        var windowWidth = window.innerWidth || $(window).width();

        if (_.respondTo === 'window') {
            respondToWidth = windowWidth;
        } else if (_.respondTo === 'slider') {
            respondToWidth = sliderWidth;
        } else if (_.respondTo === 'min') {
            respondToWidth = Math.min(windowWidth, sliderWidth);
        }

        if ( _.options.responsive &&
            _.options.responsive.length &&
            _.options.responsive !== null) {

            targetBreakpoint = null;

            for (breakpoint in _.breakpoints) {
                if (_.breakpoints.hasOwnProperty(breakpoint)) {
                    if (_.originalSettings.mobileFirst === false) {
                        if (respondToWidth < _.breakpoints[breakpoint]) {
                            targetBreakpoint = _.breakpoints[breakpoint];
                        }
                    } else {
                        if (respondToWidth > _.breakpoints[breakpoint]) {
                            targetBreakpoint = _.breakpoints[breakpoint];
                        }
                    }
                }
            }

            if (targetBreakpoint !== null) {
                if (_.activeBreakpoint !== null) {
                    if (targetBreakpoint !== _.activeBreakpoint || forceUpdate) {
                        _.activeBreakpoint =
                            targetBreakpoint;
                        if (_.breakpointSettings[targetBreakpoint] === 'unslick') {
                            _.unslick(targetBreakpoint);
                        } else {
                            _.options = $.extend({}, _.originalSettings,
                                _.breakpointSettings[
                                    targetBreakpoint]);
                            if (initial === true) {
                                _.currentSlide = _.options.initialSlide;
                            }
                            _.refresh(initial);
                        }
                        triggerBreakpoint = targetBreakpoint;
                    }
                } else {
                    _.activeBreakpoint = targetBreakpoint;
                    if (_.breakpointSettings[targetBreakpoint] === 'unslick') {
                        _.unslick(targetBreakpoint);
                    } else {
                        _.options = $.extend({}, _.originalSettings,
                            _.breakpointSettings[
                                targetBreakpoint]);
                        if (initial === true) {
                            _.currentSlide = _.options.initialSlide;
                        }
                        _.refresh(initial);
                    }
                    triggerBreakpoint = targetBreakpoint;
                }
            } else {
                if (_.activeBreakpoint !== null) {
                    _.activeBreakpoint = null;
                    _.options = _.originalSettings;
                    if (initial === true) {
                        _.currentSlide = _.options.initialSlide;
                    }
                    _.refresh(initial);
                    triggerBreakpoint = targetBreakpoint;
                }
            }

            // only trigger breakpoints during an actual break. not on initialize.
            if( !initial && triggerBreakpoint !== false ) {
                _.$slider.trigger('breakpoint', [_, triggerBreakpoint]);
            }
        }

    };

    Slick.prototype.changeSlide = function(event, dontAnimate) {

        var _ = this,
            $target = $(event.currentTarget),
            indexOffset, slideOffset, unevenOffset;

        // If target is a link, prevent default action.
        if($target.is('a')) {
            event.preventDefault();
        }

        // If target is not the <li> element (ie: a child), find the <li>.
        if(!$target.is('li')) {
            $target = $target.closest('li');
        }

        unevenOffset = (_.slideCount % _.options.slidesToScroll !== 0);
        indexOffset = unevenOffset ? 0 : (_.slideCount - _.currentSlide) % _.options.slidesToScroll;

        switch (event.data.message) {

            case 'previous':
                slideOffset = indexOffset === 0 ? _.options.slidesToScroll : _.options.slidesToShow - indexOffset;
                if (_.slideCount > _.options.slidesToShow) {
                    _.slideHandler(_.currentSlide - slideOffset, false, dontAnimate);
                }
                break;

            case 'next':
                slideOffset = indexOffset === 0 ? _.options.slidesToScroll : indexOffset;
                if (_.slideCount > _.options.slidesToShow) {
                    _.slideHandler(_.currentSlide + slideOffset, false, dontAnimate);
                }
                break;

            case 'index':
                var index = event.data.index === 0 ? 0 :
                    event.data.index || $target.index() * _.options.slidesToScroll;

                _.slideHandler(_.checkNavigable(index), false, dontAnimate);
                $target.children().trigger('focus');
                break;

            default:
                return;
        }

    };

    Slick.prototype.checkNavigable = function(index) {

        var _ = this,
            navigables, prevNavigable;

        navigables = _.getNavigableIndexes();
        prevNavigable = 0;
        if (index > navigables[navigables.length - 1]) {
            index = navigables[navigables.length - 1];
        } else {
            for (var n in navigables) {
                if (index < navigables[n]) {
                    index = prevNavigable;
                    break;
                }
                prevNavigable = navigables[n];
            }
        }

        return index;
    };

    Slick.prototype.cleanUpEvents = function() {

        var _ = this;

        if (_.options.dots && _.$dots !== null) {

            $('li', _.$dots)
                .off('click.slick', _.changeSlide)
                .off('mouseenter.slick', $.proxy(_.interrupt, _, true))
                .off('mouseleave.slick', $.proxy(_.interrupt, _, false));

            if (_.options.accessibility === true) {
                _.$dots.off('keydown.slick', _.keyHandler);
            }
        }

        _.$slider.off('focus.slick blur.slick');

        if (_.options.arrows === true && _.slideCount > _.options.slidesToShow) {
            _.$prevArrow && _.$prevArrow.off('click.slick', _.changeSlide);
            _.$nextArrow && _.$nextArrow.off('click.slick', _.changeSlide);

            if (_.options.accessibility === true) {
                _.$prevArrow && _.$prevArrow.off('keydown.slick', _.keyHandler);
                _.$nextArrow && _.$nextArrow.off('keydown.slick', _.keyHandler);
            }
        }

        _.$list.off('touchstart.slick mousedown.slick', _.swipeHandler);
        _.$list.off('touchmove.slick mousemove.slick', _.swipeHandler);
        _.$list.off('touchend.slick mouseup.slick', _.swipeHandler);
        _.$list.off('touchcancel.slick mouseleave.slick', _.swipeHandler);

        _.$list.off('click.slick', _.clickHandler);

        $(document).off(_.visibilityChange, _.visibility);

        _.cleanUpSlideEvents();

        if (_.options.accessibility === true) {
            _.$list.off('keydown.slick', _.keyHandler);
        }

        if (_.options.focusOnSelect === true) {
            $(_.$slideTrack).children().off('click.slick', _.selectHandler);
        }

        $(window).off('orientationchange.slick.slick-' + _.instanceUid, _.orientationChange);

        $(window).off('resize.slick.slick-' + _.instanceUid, _.resize);

        $('[draggable!=true]', _.$slideTrack).off('dragstart', _.preventDefault);

        $(window).off('load.slick.slick-' + _.instanceUid, _.setPosition);

    };

    Slick.prototype.cleanUpSlideEvents = function() {

        var _ = this;

        _.$list.off('mouseenter.slick', $.proxy(_.interrupt, _, true));
        _.$list.off('mouseleave.slick', $.proxy(_.interrupt, _, false));

    };

    Slick.prototype.cleanUpRows = function() {

        var _ = this, originalSlides;

        if(_.options.rows > 1) {
            originalSlides = _.$slides.children().children();
            originalSlides.removeAttr('style');
            _.$slider.empty().append(originalSlides);
        }

    };

    Slick.prototype.clickHandler = function(event) {

        var _ = this;

        if (_.shouldClick === false) {
            event.stopImmediatePropagation();
            event.stopPropagation();
            event.preventDefault();
        }

    };

    Slick.prototype.destroy = function(refresh) {

        var _ = this;

        _.autoPlayClear();

        _.touchObject = {};

        _.cleanUpEvents();

        $('.slick-cloned', _.$slider).detach();

        if (_.$dots) {
            _.$dots.remove();
        }

        if ( _.$prevArrow && _.$prevArrow.length ) {

            _.$prevArrow
                .removeClass('slick-disabled slick-arrow slick-hidden')
                .removeAttr('aria-hidden aria-disabled tabindex')
                .css('display','');

            if ( _.htmlExpr.test( _.options.prevArrow )) {
                _.$prevArrow.remove();
            }
        }

        if ( _.$nextArrow && _.$nextArrow.length ) {

            _.$nextArrow
                .removeClass('slick-disabled slick-arrow slick-hidden')
                .removeAttr('aria-hidden aria-disabled tabindex')
                .css('display','');

            if ( _.htmlExpr.test( _.options.nextArrow )) {
                _.$nextArrow.remove();
            }
        }


        if (_.$slides) {

            _.$slides
                .removeClass('slick-slide slick-active slick-center slick-visible slick-current')
                .removeAttr('aria-hidden')
                .removeAttr('data-slick-index')
                .each(function(){
                    $(this).attr('style', $(this).data('originalStyling'));
                });

            _.$slideTrack.children(this.options.slide).detach();

            _.$slideTrack.detach();

            _.$list.detach();

            _.$slider.append(_.$slides);
        }

        _.cleanUpRows();

        _.$slider.removeClass('slick-slider');
        _.$slider.removeClass('slick-initialized');
        _.$slider.removeClass('slick-dotted');

        _.unslicked = true;

        if(!refresh) {
            _.$slider.trigger('destroy', [_]);
        }

    };

    Slick.prototype.disableTransition = function(slide) {

        var _ = this,
            transition = {};

        transition[_.transitionType] = '';

        if (_.options.fade === false) {
            _.$slideTrack.css(transition);
        } else {
            _.$slides.eq(slide).css(transition);
        }

    };

    Slick.prototype.fadeSlide = function(slideIndex, callback) {

        var _ = this;

        if (_.cssTransitions === false) {

            _.$slides.eq(slideIndex).css({
                zIndex: _.options.zIndex
            });

            _.$slides.eq(slideIndex).animate({
                opacity: 1
            }, _.options.speed, _.options.easing, callback);

        } else {

            _.applyTransition(slideIndex);

            _.$slides.eq(slideIndex).css({
                opacity: 1,
                zIndex: _.options.zIndex
            });

            if (callback) {
                setTimeout(function() {

                    _.disableTransition(slideIndex);

                    callback.call();
                }, _.options.speed);
            }

        }

    };

    Slick.prototype.fadeSlideOut = function(slideIndex) {

        var _ = this;

        if (_.cssTransitions === false) {

            _.$slides.eq(slideIndex).animate({
                opacity: 0,
                zIndex: _.options.zIndex - 2
            }, _.options.speed, _.options.easing);

        } else {

            _.applyTransition(slideIndex);

            _.$slides.eq(slideIndex).css({
                opacity: 0,
                zIndex: _.options.zIndex - 2
            });

        }

    };

    Slick.prototype.filterSlides = Slick.prototype.slickFilter = function(filter) {

        var _ = this;

        if (filter !== null) {

            _.$slidesCache = _.$slides;

            _.unload();

            _.$slideTrack.children(this.options.slide).detach();

            _.$slidesCache.filter(filter).appendTo(_.$slideTrack);

            _.reinit();

        }

    };

    Slick.prototype.focusHandler = function() {

        var _ = this;

        _.$slider
            .off('focus.slick blur.slick')
            .on('focus.slick blur.slick', '*', function(event) {

            event.stopImmediatePropagation();
            var $sf = $(this);

            setTimeout(function() {

                if( _.options.pauseOnFocus ) {
                    _.focussed = $sf.is(':focus');
                    _.autoPlay();
                }

            }, 0);

        });
    };

    Slick.prototype.getCurrent = Slick.prototype.slickCurrentSlide = function() {

        var _ = this;
        return _.currentSlide;

    };

    Slick.prototype.getDotCount = function() {

        var _ = this;

        var breakPoint = 0;
        var counter = 0;
        var pagerQty = 0;

        if (_.options.infinite === true) {
            if (_.slideCount <= _.options.slidesToShow) {
                 ++pagerQty;
            } else {
                while (breakPoint < _.slideCount) {
                    ++pagerQty;
                    breakPoint = counter + _.options.slidesToScroll;
                    counter += _.options.slidesToScroll <= _.options.slidesToShow ? _.options.slidesToScroll : _.options.slidesToShow;
                }
            }
        } else if (_.options.centerMode === true) {
            pagerQty = _.slideCount;
        } else if(!_.options.asNavFor) {
            pagerQty = 1 + Math.ceil((_.slideCount - _.options.slidesToShow) / _.options.slidesToScroll);
        }else {
            while (breakPoint < _.slideCount) {
                ++pagerQty;
                breakPoint = counter + _.options.slidesToScroll;
                counter += _.options.slidesToScroll <= _.options.slidesToShow ? _.options.slidesToScroll : _.options.slidesToShow;
            }
        }

        return pagerQty - 1;

    };

    Slick.prototype.getLeft = function(slideIndex) {

        var _ = this,
            targetLeft,
            verticalHeight,
            verticalOffset = 0,
            targetSlide,
            coef;

        _.slideOffset = 0;
        verticalHeight = _.$slides.first().outerHeight(true);

        if (_.options.infinite === true) {
            if (_.slideCount > _.options.slidesToShow) {
                _.slideOffset = (_.slideWidth * _.options.slidesToShow) * -1;
                coef = -1

                if (_.options.vertical === true && _.options.centerMode === true) {
                    if (_.options.slidesToShow === 2) {
                        coef = -1.5;
                    } else if (_.options.slidesToShow === 1) {
                        coef = -2
                    }
                }
                verticalOffset = (verticalHeight * _.options.slidesToShow) * coef;
            }
            if (_.slideCount % _.options.slidesToScroll !== 0) {
                if (slideIndex + _.options.slidesToScroll > _.slideCount && _.slideCount > _.options.slidesToShow) {
                    if (slideIndex > _.slideCount) {
                        _.slideOffset = ((_.options.slidesToShow - (slideIndex - _.slideCount)) * _.slideWidth) * -1;
                        verticalOffset = ((_.options.slidesToShow - (slideIndex - _.slideCount)) * verticalHeight) * -1;
                    } else {
                        _.slideOffset = ((_.slideCount % _.options.slidesToScroll) * _.slideWidth) * -1;
                        verticalOffset = ((_.slideCount % _.options.slidesToScroll) * verticalHeight) * -1;
                    }
                }
            }
        } else {
            if (slideIndex + _.options.slidesToShow > _.slideCount) {
                _.slideOffset = ((slideIndex + _.options.slidesToShow) - _.slideCount) * _.slideWidth;
                verticalOffset = ((slideIndex + _.options.slidesToShow) - _.slideCount) * verticalHeight;
            }
        }

        if (_.slideCount <= _.options.slidesToShow) {
            _.slideOffset = 0;
            verticalOffset = 0;
        }

        if (_.options.centerMode === true && _.slideCount <= _.options.slidesToShow) {
            _.slideOffset = ((_.slideWidth * Math.floor(_.options.slidesToShow)) / 2) - ((_.slideWidth * _.slideCount) / 2);
        } else if (_.options.centerMode === true && _.options.infinite === true) {
            _.slideOffset += _.slideWidth * Math.floor(_.options.slidesToShow / 2) - _.slideWidth;
        } else if (_.options.centerMode === true) {
            _.slideOffset = 0;
            _.slideOffset += _.slideWidth * Math.floor(_.options.slidesToShow / 2);
        }

        if (_.options.vertical === false) {
            targetLeft = ((slideIndex * _.slideWidth) * -1) + _.slideOffset;
        } else {
            targetLeft = ((slideIndex * verticalHeight) * -1) + verticalOffset;
        }

        if (_.options.variableWidth === true) {

            if (_.slideCount <= _.options.slidesToShow || _.options.infinite === false) {
                targetSlide = _.$slideTrack.children('.slick-slide').eq(slideIndex);
            } else {
                targetSlide = _.$slideTrack.children('.slick-slide').eq(slideIndex + _.options.slidesToShow);
            }

            if (_.options.rtl === true) {
                if (targetSlide[0]) {
                    targetLeft = (_.$slideTrack.width() - targetSlide[0].offsetLeft - targetSlide.width()) * -1;
                } else {
                    targetLeft =  0;
                }
            } else {
                targetLeft = targetSlide[0] ? targetSlide[0].offsetLeft * -1 : 0;
            }

            if (_.options.centerMode === true) {
                if (_.slideCount <= _.options.slidesToShow || _.options.infinite === false) {
                    targetSlide = _.$slideTrack.children('.slick-slide').eq(slideIndex);
                } else {
                    targetSlide = _.$slideTrack.children('.slick-slide').eq(slideIndex + _.options.slidesToShow + 1);
                }

                if (_.options.rtl === true) {
                    if (targetSlide[0]) {
                        targetLeft = (_.$slideTrack.width() - targetSlide[0].offsetLeft - targetSlide.width()) * -1;
                    } else {
                        targetLeft =  0;
                    }
                } else {
                    targetLeft = targetSlide[0] ? targetSlide[0].offsetLeft * -1 : 0;
                }

                targetLeft += (_.$list.width() - targetSlide.outerWidth()) / 2;
            }
        }

        return targetLeft;

    };

    Slick.prototype.getOption = Slick.prototype.slickGetOption = function(option) {

        var _ = this;

        return _.options[option];

    };

    Slick.prototype.getNavigableIndexes = function() {

        var _ = this,
            breakPoint = 0,
            counter = 0,
            indexes = [],
            max;

        if (_.options.infinite === false) {
            max = _.slideCount;
        } else {
            breakPoint = _.options.slidesToScroll * -1;
            counter = _.options.slidesToScroll * -1;
            max = _.slideCount * 2;
        }

        while (breakPoint < max) {
            indexes.push(breakPoint);
            breakPoint = counter + _.options.slidesToScroll;
            counter += _.options.slidesToScroll <= _.options.slidesToShow ? _.options.slidesToScroll : _.options.slidesToShow;
        }

        return indexes;

    };

    Slick.prototype.getSlick = function() {

        return this;

    };

    Slick.prototype.getSlideCount = function() {

        var _ = this,
            slidesTraversed, swipedSlide, centerOffset;

        centerOffset = _.options.centerMode === true ? _.slideWidth * Math.floor(_.options.slidesToShow / 2) : 0;

        if (_.options.swipeToSlide === true) {
            _.$slideTrack.find('.slick-slide').each(function(index, slide) {
                if (slide.offsetLeft - centerOffset + ($(slide).outerWidth() / 2) > (_.swipeLeft * -1)) {
                    swipedSlide = slide;
                    return false;
                }
            });

            slidesTraversed = Math.abs($(swipedSlide).attr('data-slick-index') - _.currentSlide) || 1;

            return slidesTraversed;

        } else {
            return _.options.slidesToScroll;
        }

    };

    Slick.prototype.goTo = Slick.prototype.slickGoTo = function(slide, dontAnimate) {

        var _ = this;

        _.changeSlide({
            data: {
                message: 'index',
                index: parseInt(slide)
            }
        }, dontAnimate);

    };

    Slick.prototype.init = function(creation) {

        var _ = this;

        if (!$(_.$slider).hasClass('slick-initialized')) {

            $(_.$slider).addClass('slick-initialized');

            _.buildRows();
            _.buildOut();
            _.setProps();
            _.startLoad();
            _.loadSlider();
            _.initializeEvents();
            _.updateArrows();
            _.updateDots();
            _.checkResponsive(true);
            _.focusHandler();

        }

        if (creation) {
            _.$slider.trigger('init', [_]);
        }

        if (_.options.accessibility === true) {
            _.initADA();
        }

        if ( _.options.autoplay ) {

            _.paused = false;
            _.autoPlay();

        }

    };

    Slick.prototype.initADA = function() {
        var _ = this,
                numDotGroups = Math.ceil(_.slideCount / _.options.slidesToShow),
                tabControlIndexes = _.getNavigableIndexes().filter(function(val) {
                    return (val >= 0) && (val < _.slideCount);
                });

        _.$slides.add(_.$slideTrack.find('.slick-cloned')).attr({
            'aria-hidden': 'true',
            'tabindex': '-1'
        }).find('a, input, button, select').attr({
            'tabindex': '-1'
        });

        if (_.$dots !== null) {
            _.$slides.not(_.$slideTrack.find('.slick-cloned')).each(function(i) {
                var slideControlIndex = tabControlIndexes.indexOf(i);

                $(this).attr({
                    'role': 'tabpanel',
                    'id': 'slick-slide' + _.instanceUid + i,
                    'tabindex': -1
                });

                if (slideControlIndex !== -1) {
                    $(this).attr({
                        'aria-describedby': 'slick-slide-control' + _.instanceUid + slideControlIndex
                    });
                }
            });

            _.$dots.attr('role', 'tablist').find('li').each(function(i) {
                var mappedSlideIndex = tabControlIndexes[i];

                $(this).attr({
                    'role': 'presentation'
                });

                $(this).find('button').first().attr({
                    'role': 'tab',
                    'id': 'slick-slide-control' + _.instanceUid + i,
                    'aria-controls': 'slick-slide' + _.instanceUid + mappedSlideIndex,
                    'aria-label': (i + 1) + ' of ' + numDotGroups,
                    'aria-selected': null,
                    'tabindex': '-1'
                });

            }).eq(_.currentSlide).find('button').attr({
                'aria-selected': 'true',
                'tabindex': '0'
            }).end();
        }

        for (var i=_.currentSlide, max=i+_.options.slidesToShow; i < max; i++) {
            _.$slides.eq(i).attr('tabindex', 0);
        }

        _.activateADA();

    };

    Slick.prototype.initArrowEvents = function() {

        var _ = this;

        if (_.options.arrows === true && _.slideCount > _.options.slidesToShow) {
            _.$prevArrow
               .off('click.slick')
               .on('click.slick', {
                    message: 'previous'
               }, _.changeSlide);
            _.$nextArrow
               .off('click.slick')
               .on('click.slick', {
                    message: 'next'
               }, _.changeSlide);

            if (_.options.accessibility === true) {
                _.$prevArrow.on('keydown.slick', _.keyHandler);
                _.$nextArrow.on('keydown.slick', _.keyHandler);
            }
        }

    };

    Slick.prototype.initDotEvents = function() {

        var _ = this;

        if (_.options.dots === true) {
            $('li', _.$dots).on('click.slick', {
                message: 'index'
            }, _.changeSlide);

            if (_.options.accessibility === true) {
                _.$dots.on('keydown.slick', _.keyHandler);
            }
        }

        if ( _.options.dots === true && _.options.pauseOnDotsHover === true ) {

            $('li', _.$dots)
                .on('mouseenter.slick', $.proxy(_.interrupt, _, true))
                .on('mouseleave.slick', $.proxy(_.interrupt, _, false));

        }

    };

    Slick.prototype.initSlideEvents = function() {

        var _ = this;

        if ( _.options.pauseOnHover ) {

            _.$list.on('mouseenter.slick', $.proxy(_.interrupt, _, true));
            _.$list.on('mouseleave.slick', $.proxy(_.interrupt, _, false));

        }

    };

    Slick.prototype.initializeEvents = function() {

        var _ = this;

        _.initArrowEvents();

        _.initDotEvents();
        _.initSlideEvents();

        _.$list.on('touchstart.slick mousedown.slick', {
            action: 'start'
        }, _.swipeHandler);
        _.$list.on('touchmove.slick mousemove.slick', {
            action: 'move'
        }, _.swipeHandler);
        _.$list.on('touchend.slick mouseup.slick', {
            action: 'end'
        }, _.swipeHandler);
        _.$list.on('touchcancel.slick mouseleave.slick', {
            action: 'end'
        }, _.swipeHandler);

        _.$list.on('click.slick', _.clickHandler);

        $(document).on(_.visibilityChange, $.proxy(_.visibility, _));

        if (_.options.accessibility === true) {
            _.$list.on('keydown.slick', _.keyHandler);
        }

        if (_.options.focusOnSelect === true) {
            $(_.$slideTrack).children().on('click.slick', _.selectHandler);
        }

        $(window).on('orientationchange.slick.slick-' + _.instanceUid, $.proxy(_.orientationChange, _));

        $(window).on('resize.slick.slick-' + _.instanceUid, $.proxy(_.resize, _));

        $('[draggable!=true]', _.$slideTrack).on('dragstart', _.preventDefault);

        $(window).on('load.slick.slick-' + _.instanceUid, _.setPosition);
        $(_.setPosition);

    };

    Slick.prototype.initUI = function() {

        var _ = this;

        if (_.options.arrows === true && _.slideCount > _.options.slidesToShow) {

            _.$prevArrow.show();
            _.$nextArrow.show();

        }

        if (_.options.dots === true && _.slideCount > _.options.slidesToShow) {

            _.$dots.show();

        }

    };

    Slick.prototype.keyHandler = function(event) {

        var _ = this;
         //Dont slide if the cursor is inside the form fields and arrow keys are pressed
        if(!event.target.tagName.match('TEXTAREA|INPUT|SELECT')) {
            if (event.keyCode === 37 && _.options.accessibility === true) {
                _.changeSlide({
                    data: {
                        message: _.options.rtl === true ? 'next' :  'previous'
                    }
                });
            } else if (event.keyCode === 39 && _.options.accessibility === true) {
                _.changeSlide({
                    data: {
                        message: _.options.rtl === true ? 'previous' : 'next'
                    }
                });
            }
        }

    };

    Slick.prototype.lazyLoad = function() {

        var _ = this,
            loadRange, cloneRange, rangeStart, rangeEnd;

        function loadImages(imagesScope) {

            $('img[data-lazy]', imagesScope).each(function() {

                var image = $(this),
                    imageSource = $(this).attr('data-lazy'),
                    imageSrcSet = $(this).attr('data-srcset'),
                    imageSizes  = $(this).attr('data-sizes') || _.$slider.attr('data-sizes'),
                    imageToLoad = document.createElement('img');

                imageToLoad.onload = function() {

                    image
                        .animate({ opacity: 0 }, 100, function() {

                            if (imageSrcSet) {
                                image
                                    .attr('srcset', imageSrcSet );

                                if (imageSizes) {
                                    image
                                        .attr('sizes', imageSizes );
                                }
                            }

                            image
                                .attr('src', imageSource)
                                .animate({ opacity: 1 }, 200, function() {
                                    image
                                        .removeAttr('data-lazy data-srcset data-sizes')
                                        .removeClass('slick-loading');
                                });
                            _.$slider.trigger('lazyLoaded', [_, image, imageSource]);
                        });

                };

                imageToLoad.onerror = function() {

                    image
                        .removeAttr( 'data-lazy' )
                        .removeClass( 'slick-loading' )
                        .addClass( 'slick-lazyload-error' );

                    _.$slider.trigger('lazyLoadError', [ _, image, imageSource ]);

                };

                imageToLoad.src = imageSource;

            });

        }

        if (_.options.centerMode === true) {
            if (_.options.infinite === true) {
                rangeStart = _.currentSlide + (_.options.slidesToShow / 2 + 1);
                rangeEnd = rangeStart + _.options.slidesToShow + 2;
            } else {
                rangeStart = Math.max(0, _.currentSlide - (_.options.slidesToShow / 2 + 1));
                rangeEnd = 2 + (_.options.slidesToShow / 2 + 1) + _.currentSlide;
            }
        } else {
            rangeStart = _.options.infinite ? _.options.slidesToShow + _.currentSlide : _.currentSlide;
            rangeEnd = Math.ceil(rangeStart + _.options.slidesToShow);
            if (_.options.fade === true) {
                if (rangeStart > 0) rangeStart--;
                if (rangeEnd <= _.slideCount) rangeEnd++;
            }
        }

        loadRange = _.$slider.find('.slick-slide').slice(rangeStart, rangeEnd);

        if (_.options.lazyLoad === 'anticipated') {
            var prevSlide = rangeStart - 1,
                nextSlide = rangeEnd,
                $slides = _.$slider.find('.slick-slide');

            for (var i = 0; i < _.options.slidesToScroll; i++) {
                if (prevSlide < 0) prevSlide = _.slideCount - 1;
                loadRange = loadRange.add($slides.eq(prevSlide));
                loadRange = loadRange.add($slides.eq(nextSlide));
                prevSlide--;
                nextSlide++;
            }
        }

        loadImages(loadRange);

        if (_.slideCount <= _.options.slidesToShow) {
            cloneRange = _.$slider.find('.slick-slide');
            loadImages(cloneRange);
        } else
        if (_.currentSlide >= _.slideCount - _.options.slidesToShow) {
            cloneRange = _.$slider.find('.slick-cloned').slice(0, _.options.slidesToShow);
            loadImages(cloneRange);
        } else if (_.currentSlide === 0) {
            cloneRange = _.$slider.find('.slick-cloned').slice(_.options.slidesToShow * -1);
            loadImages(cloneRange);
        }

    };

    Slick.prototype.loadSlider = function() {

        var _ = this;

        _.setPosition();

        _.$slideTrack.css({
            opacity: 1
        });

        _.$slider.removeClass('slick-loading');

        _.initUI();

        if (_.options.lazyLoad === 'progressive') {
            _.progressiveLazyLoad();
        }

    };

    Slick.prototype.next = Slick.prototype.slickNext = function() {

        var _ = this;

        _.changeSlide({
            data: {
                message: 'next'
            }
        });

    };

    Slick.prototype.orientationChange = function() {

        var _ = this;

        _.checkResponsive();
        _.setPosition();

    };

    Slick.prototype.pause = Slick.prototype.slickPause = function() {

        var _ = this;

        _.autoPlayClear();
        _.paused = true;

    };

    Slick.prototype.play = Slick.prototype.slickPlay = function() {

        var _ = this;

        _.autoPlay();
        _.options.autoplay = true;
        _.paused = false;
        _.focussed = false;
        _.interrupted = false;

    };

    Slick.prototype.postSlide = function(index) {

        var _ = this;

        if( !_.unslicked ) {

            _.$slider.trigger('afterChange', [_, index]);

            _.animating = false;

            if (_.slideCount > _.options.slidesToShow) {
                _.setPosition();
            }

            _.swipeLeft = null;

            if ( _.options.autoplay ) {
                _.autoPlay();
            }

            if (_.options.accessibility === true) {
                _.initADA();
                
                if (_.options.focusOnChange) {
                    var $currentSlide = $(_.$slides.get(_.currentSlide));
                    $currentSlide.attr('tabindex', 0).focus();
                }
            }

        }

    };

    Slick.prototype.prev = Slick.prototype.slickPrev = function() {

        var _ = this;

        _.changeSlide({
            data: {
                message: 'previous'
            }
        });

    };

    Slick.prototype.preventDefault = function(event) {

        event.preventDefault();

    };

    Slick.prototype.progressiveLazyLoad = function( tryCount ) {

        tryCount = tryCount || 1;

        var _ = this,
            $imgsToLoad = $( 'img[data-lazy]', _.$slider ),
            image,
            imageSource,
            imageSrcSet,
            imageSizes,
            imageToLoad;

        if ( $imgsToLoad.length ) {

            image = $imgsToLoad.first();
            imageSource = image.attr('data-lazy');
            imageSrcSet = image.attr('data-srcset');
            imageSizes  = image.attr('data-sizes') || _.$slider.attr('data-sizes');
            imageToLoad = document.createElement('img');

            imageToLoad.onload = function() {

                if (imageSrcSet) {
                    image
                        .attr('srcset', imageSrcSet );

                    if (imageSizes) {
                        image
                            .attr('sizes', imageSizes );
                    }
                }

                image
                    .attr( 'src', imageSource )
                    .removeAttr('data-lazy data-srcset data-sizes')
                    .removeClass('slick-loading');

                if ( _.options.adaptiveHeight === true ) {
                    _.setPosition();
                }

                _.$slider.trigger('lazyLoaded', [ _, image, imageSource ]);
                _.progressiveLazyLoad();

            };

            imageToLoad.onerror = function() {

                if ( tryCount < 3 ) {

                    /**
                     * try to load the image 3 times,
                     * leave a slight delay so we don't get
                     * servers blocking the request.
                     */
                    setTimeout( function() {
                        _.progressiveLazyLoad( tryCount + 1 );
                    }, 500 );

                } else {

                    image
                        .removeAttr( 'data-lazy' )
                        .removeClass( 'slick-loading' )
                        .addClass( 'slick-lazyload-error' );

                    _.$slider.trigger('lazyLoadError', [ _, image, imageSource ]);

                    _.progressiveLazyLoad();

                }

            };

            imageToLoad.src = imageSource;

        } else {

            _.$slider.trigger('allImagesLoaded', [ _ ]);

        }

    };

    Slick.prototype.refresh = function( initializing ) {

        var _ = this, currentSlide, lastVisibleIndex;

        lastVisibleIndex = _.slideCount - _.options.slidesToShow;

        // in non-infinite sliders, we don't want to go past the
        // last visible index.
        if( !_.options.infinite && ( _.currentSlide > lastVisibleIndex )) {
            _.currentSlide = lastVisibleIndex;
        }

        // if less slides than to show, go to start.
        if ( _.slideCount <= _.options.slidesToShow ) {
            _.currentSlide = 0;

        }

        currentSlide = _.currentSlide;

        _.destroy(true);

        $.extend(_, _.initials, { currentSlide: currentSlide });

        _.init();

        if( !initializing ) {

            _.changeSlide({
                data: {
                    message: 'index',
                    index: currentSlide
                }
            }, false);

        }

    };

    Slick.prototype.registerBreakpoints = function() {

        var _ = this, breakpoint, currentBreakpoint, l,
            responsiveSettings = _.options.responsive || null;

        if ( $.type(responsiveSettings) === 'array' && responsiveSettings.length ) {

            _.respondTo = _.options.respondTo || 'window';

            for ( breakpoint in responsiveSettings ) {

                l = _.breakpoints.length-1;

                if (responsiveSettings.hasOwnProperty(breakpoint)) {
                    currentBreakpoint = responsiveSettings[breakpoint].breakpoint;

                    // loop through the breakpoints and cut out any existing
                    // ones with the same breakpoint number, we don't want dupes.
                    while( l >= 0 ) {
                        if( _.breakpoints[l] && _.breakpoints[l] === currentBreakpoint ) {
                            _.breakpoints.splice(l,1);
                        }
                        l--;
                    }

                    _.breakpoints.push(currentBreakpoint);
                    _.breakpointSettings[currentBreakpoint] = responsiveSettings[breakpoint].settings;

                }

            }

            _.breakpoints.sort(function(a, b) {
                return ( _.options.mobileFirst ) ? a-b : b-a;
            });

        }

    };

    Slick.prototype.reinit = function() {

        var _ = this;

        _.$slides =
            _.$slideTrack
                .children(_.options.slide)
                .addClass('slick-slide');

        _.slideCount = _.$slides.length;

        if (_.currentSlide >= _.slideCount && _.currentSlide !== 0) {
            _.currentSlide = _.currentSlide - _.options.slidesToScroll;
        }

        if (_.slideCount <= _.options.slidesToShow) {
            _.currentSlide = 0;
        }

        _.registerBreakpoints();

        _.setProps();
        _.setupInfinite();
        _.buildArrows();
        _.updateArrows();
        _.initArrowEvents();
        _.buildDots();
        _.updateDots();
        _.initDotEvents();
        _.cleanUpSlideEvents();
        _.initSlideEvents();

        _.checkResponsive(false, true);

        if (_.options.focusOnSelect === true) {
            $(_.$slideTrack).children().on('click.slick', _.selectHandler);
        }

        _.setSlideClasses(typeof _.currentSlide === 'number' ? _.currentSlide : 0);

        _.setPosition();
        _.focusHandler();

        _.paused = !_.options.autoplay;
        _.autoPlay();

        _.$slider.trigger('reInit', [_]);

    };

    Slick.prototype.resize = function() {

        var _ = this;

        if ($(window).width() !== _.windowWidth) {
            clearTimeout(_.windowDelay);
            _.windowDelay = window.setTimeout(function() {
                _.windowWidth = $(window).width();
                _.checkResponsive();
                if( !_.unslicked ) { _.setPosition(); }
            }, 50);
        }
    };

    Slick.prototype.removeSlide = Slick.prototype.slickRemove = function(index, removeBefore, removeAll) {

        var _ = this;

        if (typeof(index) === 'boolean') {
            removeBefore = index;
            index = removeBefore === true ? 0 : _.slideCount - 1;
        } else {
            index = removeBefore === true ? --index : index;
        }

        if (_.slideCount < 1 || index < 0 || index > _.slideCount - 1) {
            return false;
        }

        _.unload();

        if (removeAll === true) {
            _.$slideTrack.children().remove();
        } else {
            _.$slideTrack.children(this.options.slide).eq(index).remove();
        }

        _.$slides = _.$slideTrack.children(this.options.slide);

        _.$slideTrack.children(this.options.slide).detach();

        _.$slideTrack.append(_.$slides);

        _.$slidesCache = _.$slides;

        _.reinit();

    };

    Slick.prototype.setCSS = function(position) {

        var _ = this,
            positionProps = {},
            x, y;

        if (_.options.rtl === true) {
            position = -position;
        }
        x = _.positionProp == 'left' ? Math.ceil(position) + 'px' : '0px';
        y = _.positionProp == 'top' ? Math.ceil(position) + 'px' : '0px';

        positionProps[_.positionProp] = position;

        if (_.transformsEnabled === false) {
            _.$slideTrack.css(positionProps);
        } else {
            positionProps = {};
            if (_.cssTransitions === false) {
                positionProps[_.animType] = 'translate(' + x + ', ' + y + ')';
                _.$slideTrack.css(positionProps);
            } else {
                positionProps[_.animType] = 'translate3d(' + x + ', ' + y + ', 0px)';
                _.$slideTrack.css(positionProps);
            }
        }

    };

    Slick.prototype.setDimensions = function() {

        var _ = this;

        if (_.options.vertical === false) {
            if (_.options.centerMode === true) {
                _.$list.css({
                    padding: ('0px ' + _.options.centerPadding)
                });
            }
        } else {
            _.$list.height(_.$slides.first().outerHeight(true) * _.options.slidesToShow);
            if (_.options.centerMode === true) {
                _.$list.css({
                    padding: (_.options.centerPadding + ' 0px')
                });
            }
        }

        _.listWidth = _.$list.width();
        _.listHeight = _.$list.height();


        if (_.options.vertical === false && _.options.variableWidth === false) {
            _.slideWidth = Math.ceil(_.listWidth / _.options.slidesToShow);
            _.$slideTrack.width(Math.ceil((_.slideWidth * _.$slideTrack.children('.slick-slide').length)));

        } else if (_.options.variableWidth === true) {
            _.$slideTrack.width(5000 * _.slideCount);
        } else {
            _.slideWidth = Math.ceil(_.listWidth);
            _.$slideTrack.height(Math.ceil((_.$slides.first().outerHeight(true) * _.$slideTrack.children('.slick-slide').length)));
        }

        var offset = _.$slides.first().outerWidth(true) - _.$slides.first().width();
        if (_.options.variableWidth === false) _.$slideTrack.children('.slick-slide').width(_.slideWidth - offset);

    };

    Slick.prototype.setFade = function() {

        var _ = this,
            targetLeft;

        _.$slides.each(function(index, element) {
            targetLeft = (_.slideWidth * index) * -1;
            if (_.options.rtl === true) {
                $(element).css({
                    position: 'relative',
                    right: targetLeft,
                    top: 0,
                    zIndex: _.options.zIndex - 2,
                    opacity: 0
                });
            } else {
                $(element).css({
                    position: 'relative',
                    left: targetLeft,
                    top: 0,
                    zIndex: _.options.zIndex - 2,
                    opacity: 0
                });
            }
        });

        _.$slides.eq(_.currentSlide).css({
            zIndex: _.options.zIndex - 1,
            opacity: 1
        });

    };

    Slick.prototype.setHeight = function() {

        var _ = this;

        if (_.options.slidesToShow === 1 && _.options.adaptiveHeight === true && _.options.vertical === false) {
            var targetHeight = _.$slides.eq(_.currentSlide).outerHeight(true);
            _.$list.css('height', targetHeight);
        }

    };

    Slick.prototype.setOption =
    Slick.prototype.slickSetOption = function() {

        /**
         * accepts arguments in format of:
         *
         *  - for changing a single option's value:
         *     .slick("setOption", option, value, refresh )
         *
         *  - for changing a set of responsive options:
         *     .slick("setOption", 'responsive', [{}, ...], refresh )
         *
         *  - for updating multiple values at once (not responsive)
         *     .slick("setOption", { 'option': value, ... }, refresh )
         */

        var _ = this, l, item, option, value, refresh = false, type;

        if( $.type( arguments[0] ) === 'object' ) {

            option =  arguments[0];
            refresh = arguments[1];
            type = 'multiple';

        } else if ( $.type( arguments[0] ) === 'string' ) {

            option =  arguments[0];
            value = arguments[1];
            refresh = arguments[2];

            if ( arguments[0] === 'responsive' && $.type( arguments[1] ) === 'array' ) {

                type = 'responsive';

            } else if ( typeof arguments[1] !== 'undefined' ) {

                type = 'single';

            }

        }

        if ( type === 'single' ) {

            _.options[option] = value;


        } else if ( type === 'multiple' ) {

            $.each( option , function( opt, val ) {

                _.options[opt] = val;

            });


        } else if ( type === 'responsive' ) {

            for ( item in value ) {

                if( $.type( _.options.responsive ) !== 'array' ) {

                    _.options.responsive = [ value[item] ];

                } else {

                    l = _.options.responsive.length-1;

                    // loop through the responsive object and splice out duplicates.
                    while( l >= 0 ) {

                        if( _.options.responsive[l].breakpoint === value[item].breakpoint ) {

                            _.options.responsive.splice(l,1);

                        }

                        l--;

                    }

                    _.options.responsive.push( value[item] );

                }

            }

        }

        if ( refresh ) {

            _.unload();
            _.reinit();

        }

    };

    Slick.prototype.setPosition = function() {

        var _ = this;

        _.setDimensions();

        _.setHeight();

        if (_.options.fade === false) {
            _.setCSS(_.getLeft(_.currentSlide));
        } else {
            _.setFade();
        }

        _.$slider.trigger('setPosition', [_]);

    };

    Slick.prototype.setProps = function() {

        var _ = this,
            bodyStyle = document.body.style;

        _.positionProp = _.options.vertical === true ? 'top' : 'left';

        if (_.positionProp === 'top') {
            _.$slider.addClass('slick-vertical');
        } else {
            _.$slider.removeClass('slick-vertical');
        }

        if (bodyStyle.WebkitTransition !== undefined ||
            bodyStyle.MozTransition !== undefined ||
            bodyStyle.msTransition !== undefined) {
            if (_.options.useCSS === true) {
                _.cssTransitions = true;
            }
        }

        if ( _.options.fade ) {
            if ( typeof _.options.zIndex === 'number' ) {
                if( _.options.zIndex < 3 ) {
                    _.options.zIndex = 3;
                }
            } else {
                _.options.zIndex = _.defaults.zIndex;
            }
        }

        if (bodyStyle.OTransform !== undefined) {
            _.animType = 'OTransform';
            _.transformType = '-o-transform';
            _.transitionType = 'OTransition';
            if (bodyStyle.perspectiveProperty === undefined && bodyStyle.webkitPerspective === undefined) _.animType = false;
        }
        if (bodyStyle.MozTransform !== undefined) {
            _.animType = 'MozTransform';
            _.transformType = '-moz-transform';
            _.transitionType = 'MozTransition';
            if (bodyStyle.perspectiveProperty === undefined && bodyStyle.MozPerspective === undefined) _.animType = false;
        }
        if (bodyStyle.webkitTransform !== undefined) {
            _.animType = 'webkitTransform';
            _.transformType = '-webkit-transform';
            _.transitionType = 'webkitTransition';
            if (bodyStyle.perspectiveProperty === undefined && bodyStyle.webkitPerspective === undefined) _.animType = false;
        }
        if (bodyStyle.msTransform !== undefined) {
            _.animType = 'msTransform';
            _.transformType = '-ms-transform';
            _.transitionType = 'msTransition';
            if (bodyStyle.msTransform === undefined) _.animType = false;
        }
        if (bodyStyle.transform !== undefined && _.animType !== false) {
            _.animType = 'transform';
            _.transformType = 'transform';
            _.transitionType = 'transition';
        }
        _.transformsEnabled = _.options.useTransform && (_.animType !== null && _.animType !== false);
    };


    Slick.prototype.setSlideClasses = function(index) {

        var _ = this,
            centerOffset, allSlides, indexOffset, remainder;

        allSlides = _.$slider
            .find('.slick-slide')
            .removeClass('slick-active slick-center slick-current')
            .attr('aria-hidden', 'true');

        _.$slides
            .eq(index)
            .addClass('slick-current');

        if (_.options.centerMode === true) {

            var evenCoef = _.options.slidesToShow % 2 === 0 ? 1 : 0;

            centerOffset = Math.floor(_.options.slidesToShow / 2);

            if (_.options.infinite === true) {

                if (index >= centerOffset && index <= (_.slideCount - 1) - centerOffset) {
                    _.$slides
                        .slice(index - centerOffset + evenCoef, index + centerOffset + 1)
                        .addClass('slick-active')
                        .attr('aria-hidden', 'false');

                } else {

                    indexOffset = _.options.slidesToShow + index;
                    allSlides
                        .slice(indexOffset - centerOffset + 1 + evenCoef, indexOffset + centerOffset + 2)
                        .addClass('slick-active')
                        .attr('aria-hidden', 'false');

                }

                if (index === 0) {

                    allSlides
                        .eq(allSlides.length - 1 - _.options.slidesToShow)
                        .addClass('slick-center');

                } else if (index === _.slideCount - 1) {

                    allSlides
                        .eq(_.options.slidesToShow)
                        .addClass('slick-center');

                }

            }

            _.$slides
                .eq(index)
                .addClass('slick-center');

        } else {

            if (index >= 0 && index <= (_.slideCount - _.options.slidesToShow)) {

                _.$slides
                    .slice(index, index + _.options.slidesToShow)
                    .addClass('slick-active')
                    .attr('aria-hidden', 'false');

            } else if (allSlides.length <= _.options.slidesToShow) {

                allSlides
                    .addClass('slick-active')
                    .attr('aria-hidden', 'false');

            } else {

                remainder = _.slideCount % _.options.slidesToShow;
                indexOffset = _.options.infinite === true ? _.options.slidesToShow + index : index;

                if (_.options.slidesToShow == _.options.slidesToScroll && (_.slideCount - index) < _.options.slidesToShow) {

                    allSlides
                        .slice(indexOffset - (_.options.slidesToShow - remainder), indexOffset + remainder)
                        .addClass('slick-active')
                        .attr('aria-hidden', 'false');

                } else {

                    allSlides
                        .slice(indexOffset, indexOffset + _.options.slidesToShow)
                        .addClass('slick-active')
                        .attr('aria-hidden', 'false');

                }

            }

        }

        if (_.options.lazyLoad === 'ondemand' || _.options.lazyLoad === 'anticipated') {
            _.lazyLoad();
        }
    };

    Slick.prototype.setupInfinite = function() {

        var _ = this,
            i, slideIndex, infiniteCount;

        if (_.options.fade === true) {
            _.options.centerMode = false;
        }

        if (_.options.infinite === true && _.options.fade === false) {

            slideIndex = null;

            if (_.slideCount > _.options.slidesToShow) {

                if (_.options.centerMode === true) {
                    infiniteCount = _.options.slidesToShow + 1;
                } else {
                    infiniteCount = _.options.slidesToShow;
                }

                for (i = _.slideCount; i > (_.slideCount -
                        infiniteCount); i -= 1) {
                    slideIndex = i - 1;
                    $(_.$slides[slideIndex]).clone(true).attr('id', '')
                        .attr('data-slick-index', slideIndex - _.slideCount)
                        .prependTo(_.$slideTrack).addClass('slick-cloned');
                }
                for (i = 0; i < infiniteCount  + _.slideCount; i += 1) {
                    slideIndex = i;
                    $(_.$slides[slideIndex]).clone(true).attr('id', '')
                        .attr('data-slick-index', slideIndex + _.slideCount)
                        .appendTo(_.$slideTrack).addClass('slick-cloned');
                }
                _.$slideTrack.find('.slick-cloned').find('[id]').each(function() {
                    $(this).attr('id', '');
                });

            }

        }

    };

    Slick.prototype.interrupt = function( toggle ) {

        var _ = this;

        if( !toggle ) {
            _.autoPlay();
        }
        _.interrupted = toggle;

    };

    Slick.prototype.selectHandler = function(event) {

        var _ = this;

        var targetElement =
            $(event.target).is('.slick-slide') ?
                $(event.target) :
                $(event.target).parents('.slick-slide');

        var index = parseInt(targetElement.attr('data-slick-index'));

        if (!index) index = 0;

        if (_.slideCount <= _.options.slidesToShow) {

            _.slideHandler(index, false, true);
            return;

        }

        _.slideHandler(index);

    };

    Slick.prototype.slideHandler = function(index, sync, dontAnimate) {

        var targetSlide, animSlide, oldSlide, slideLeft, targetLeft = null,
            _ = this, navTarget;

        sync = sync || false;

        if (_.animating === true && _.options.waitForAnimate === true) {
            return;
        }

        if (_.options.fade === true && _.currentSlide === index) {
            return;
        }

        if (sync === false) {
            _.asNavFor(index);
        }

        targetSlide = index;
        targetLeft = _.getLeft(targetSlide);
        slideLeft = _.getLeft(_.currentSlide);

        _.currentLeft = _.swipeLeft === null ? slideLeft : _.swipeLeft;

        if (_.options.infinite === false && _.options.centerMode === false && (index < 0 || index > _.getDotCount() * _.options.slidesToScroll)) {
            if (_.options.fade === false) {
                targetSlide = _.currentSlide;
                if (dontAnimate !== true) {
                    _.animateSlide(slideLeft, function() {
                        _.postSlide(targetSlide);
                    });
                } else {
                    _.postSlide(targetSlide);
                }
            }
            return;
        } else if (_.options.infinite === false && _.options.centerMode === true && (index < 0 || index > (_.slideCount - _.options.slidesToScroll))) {
            if (_.options.fade === false) {
                targetSlide = _.currentSlide;
                if (dontAnimate !== true) {
                    _.animateSlide(slideLeft, function() {
                        _.postSlide(targetSlide);
                    });
                } else {
                    _.postSlide(targetSlide);
                }
            }
            return;
        }

        if ( _.options.autoplay ) {
            clearInterval(_.autoPlayTimer);
        }

        if (targetSlide < 0) {
            if (_.slideCount % _.options.slidesToScroll !== 0) {
                animSlide = _.slideCount - (_.slideCount % _.options.slidesToScroll);
            } else {
                animSlide = _.slideCount + targetSlide;
            }
        } else if (targetSlide >= _.slideCount) {
            if (_.slideCount % _.options.slidesToScroll !== 0) {
                animSlide = 0;
            } else {
                animSlide = targetSlide - _.slideCount;
            }
        } else {
            animSlide = targetSlide;
        }

        _.animating = true;

        _.$slider.trigger('beforeChange', [_, _.currentSlide, animSlide]);

        oldSlide = _.currentSlide;
        _.currentSlide = animSlide;

        _.setSlideClasses(_.currentSlide);

        if ( _.options.asNavFor ) {

            navTarget = _.getNavTarget();
            navTarget = navTarget.slick('getSlick');

            if ( navTarget.slideCount <= navTarget.options.slidesToShow ) {
                navTarget.setSlideClasses(_.currentSlide);
            }

        }

        _.updateDots();
        _.updateArrows();

        if (_.options.fade === true) {
            if (dontAnimate !== true) {

                _.fadeSlideOut(oldSlide);

                _.fadeSlide(animSlide, function() {
                    _.postSlide(animSlide);
                });

            } else {
                _.postSlide(animSlide);
            }
            _.animateHeight();
            return;
        }

        if (dontAnimate !== true) {
            _.animateSlide(targetLeft, function() {
                _.postSlide(animSlide);
            });
        } else {
            _.postSlide(animSlide);
        }

    };

    Slick.prototype.startLoad = function() {

        var _ = this;

        if (_.options.arrows === true && _.slideCount > _.options.slidesToShow) {

            _.$prevArrow.hide();
            _.$nextArrow.hide();

        }

        if (_.options.dots === true && _.slideCount > _.options.slidesToShow) {

            _.$dots.hide();

        }

        _.$slider.addClass('slick-loading');

    };

    Slick.prototype.swipeDirection = function() {

        var xDist, yDist, r, swipeAngle, _ = this;

        xDist = _.touchObject.startX - _.touchObject.curX;
        yDist = _.touchObject.startY - _.touchObject.curY;
        r = Math.atan2(yDist, xDist);

        swipeAngle = Math.round(r * 180 / Math.PI);
        if (swipeAngle < 0) {
            swipeAngle = 360 - Math.abs(swipeAngle);
        }

        if ((swipeAngle <= 45) && (swipeAngle >= 0)) {
            return (_.options.rtl === false ? 'left' : 'right');
        }
        if ((swipeAngle <= 360) && (swipeAngle >= 315)) {
            return (_.options.rtl === false ? 'left' : 'right');
        }
        if ((swipeAngle >= 135) && (swipeAngle <= 225)) {
            return (_.options.rtl === false ? 'right' : 'left');
        }
        if (_.options.verticalSwiping === true) {
            if ((swipeAngle >= 35) && (swipeAngle <= 135)) {
                return 'down';
            } else {
                return 'up';
            }
        }

        return 'vertical';

    };

    Slick.prototype.swipeEnd = function(event) {

        var _ = this,
            slideCount,
            direction;

        _.dragging = false;
        _.swiping = false;

        if (_.scrolling) {
            _.scrolling = false;
            return false;
        }

        _.interrupted = false;
        _.shouldClick = ( _.touchObject.swipeLength > 10 ) ? false : true;

        if ( _.touchObject.curX === undefined ) {
            return false;
        }

        if ( _.touchObject.edgeHit === true ) {
            _.$slider.trigger('edge', [_, _.swipeDirection() ]);
        }

        if ( _.touchObject.swipeLength >= _.touchObject.minSwipe ) {

            direction = _.swipeDirection();

            switch ( direction ) {

                case 'left':
                case 'down':

                    slideCount =
                        _.options.swipeToSlide ?
                            _.checkNavigable( _.currentSlide + _.getSlideCount() ) :
                            _.currentSlide + _.getSlideCount();

                    _.currentDirection = 0;

                    break;

                case 'right':
                case 'up':

                    slideCount =
                        _.options.swipeToSlide ?
                            _.checkNavigable( _.currentSlide - _.getSlideCount() ) :
                            _.currentSlide - _.getSlideCount();

                    _.currentDirection = 1;

                    break;

                default:


            }

            if( direction != 'vertical' ) {

                _.slideHandler( slideCount );
                _.touchObject = {};
                _.$slider.trigger('swipe', [_, direction ]);

            }

        } else {

            if ( _.touchObject.startX !== _.touchObject.curX ) {

                _.slideHandler( _.currentSlide );
                _.touchObject = {};

            }

        }

    };

    Slick.prototype.swipeHandler = function(event) {

        var _ = this;

        if ((_.options.swipe === false) || ('ontouchend' in document && _.options.swipe === false)) {
            return;
        } else if (_.options.draggable === false && event.type.indexOf('mouse') !== -1) {
            return;
        }

        _.touchObject.fingerCount = event.originalEvent && event.originalEvent.touches !== undefined ?
            event.originalEvent.touches.length : 1;

        _.touchObject.minSwipe = _.listWidth / _.options
            .touchThreshold;

        if (_.options.verticalSwiping === true) {
            _.touchObject.minSwipe = _.listHeight / _.options
                .touchThreshold;
        }

        switch (event.data.action) {

            case 'start':
                _.swipeStart(event);
                break;

            case 'move':
                _.swipeMove(event);
                break;

            case 'end':
                _.swipeEnd(event);
                break;

        }

    };

    Slick.prototype.swipeMove = function(event) {

        var _ = this,
            edgeWasHit = false,
            curLeft, swipeDirection, swipeLength, positionOffset, touches, verticalSwipeLength;

        touches = event.originalEvent !== undefined ? event.originalEvent.touches : null;

        if (!_.dragging || _.scrolling || touches && touches.length !== 1) {
            return false;
        }

        curLeft = _.getLeft(_.currentSlide);

        _.touchObject.curX = touches !== undefined ? touches[0].pageX : event.clientX;
        _.touchObject.curY = touches !== undefined ? touches[0].pageY : event.clientY;

        _.touchObject.swipeLength = Math.round(Math.sqrt(
            Math.pow(_.touchObject.curX - _.touchObject.startX, 2)));

        verticalSwipeLength = Math.round(Math.sqrt(
            Math.pow(_.touchObject.curY - _.touchObject.startY, 2)));

        if (!_.options.verticalSwiping && !_.swiping && verticalSwipeLength > 4) {
            _.scrolling = true;
            return false;
        }

        if (_.options.verticalSwiping === true) {
            _.touchObject.swipeLength = verticalSwipeLength;
        }

        swipeDirection = _.swipeDirection();

        if (event.originalEvent !== undefined && _.touchObject.swipeLength > 4) {
            _.swiping = true;
            event.preventDefault();
        }

        positionOffset = (_.options.rtl === false ? 1 : -1) * (_.touchObject.curX > _.touchObject.startX ? 1 : -1);
        if (_.options.verticalSwiping === true) {
            positionOffset = _.touchObject.curY > _.touchObject.startY ? 1 : -1;
        }


        swipeLength = _.touchObject.swipeLength;

        _.touchObject.edgeHit = false;

        if (_.options.infinite === false) {
            if ((_.currentSlide === 0 && swipeDirection === 'right') || (_.currentSlide >= _.getDotCount() && swipeDirection === 'left')) {
                swipeLength = _.touchObject.swipeLength * _.options.edgeFriction;
                _.touchObject.edgeHit = true;
            }
        }

        if (_.options.vertical === false) {
            _.swipeLeft = curLeft + swipeLength * positionOffset;
        } else {
            _.swipeLeft = curLeft + (swipeLength * (_.$list.height() / _.listWidth)) * positionOffset;
        }
        if (_.options.verticalSwiping === true) {
            _.swipeLeft = curLeft + swipeLength * positionOffset;
        }

        if (_.options.fade === true || _.options.touchMove === false) {
            return false;
        }

        if (_.animating === true) {
            _.swipeLeft = null;
            return false;
        }

        _.setCSS(_.swipeLeft);

    };

    Slick.prototype.swipeStart = function(event) {

        var _ = this,
            touches;

        _.interrupted = true;

        if (_.touchObject.fingerCount !== 1 || _.slideCount <= _.options.slidesToShow) {
            _.touchObject = {};
            return false;
        }

        if (event.originalEvent !== undefined && event.originalEvent.touches !== undefined) {
            touches = event.originalEvent.touches[0];
        }

        _.touchObject.startX = _.touchObject.curX = touches !== undefined ? touches.pageX : event.clientX;
        _.touchObject.startY = _.touchObject.curY = touches !== undefined ? touches.pageY : event.clientY;

        _.dragging = true;

    };

    Slick.prototype.unfilterSlides = Slick.prototype.slickUnfilter = function() {

        var _ = this;

        if (_.$slidesCache !== null) {

            _.unload();

            _.$slideTrack.children(this.options.slide).detach();

            _.$slidesCache.appendTo(_.$slideTrack);

            _.reinit();

        }

    };

    Slick.prototype.unload = function() {

        var _ = this;

        $('.slick-cloned', _.$slider).remove();

        if (_.$dots) {
            _.$dots.remove();
        }

        if (_.$prevArrow && _.htmlExpr.test(_.options.prevArrow)) {
            _.$prevArrow.remove();
        }

        if (_.$nextArrow && _.htmlExpr.test(_.options.nextArrow)) {
            _.$nextArrow.remove();
        }

        _.$slides
            .removeClass('slick-slide slick-active slick-visible slick-current')
            .attr('aria-hidden', 'true')
            .css('width', '');

    };

    Slick.prototype.unslick = function(fromBreakpoint) {

        var _ = this;
        _.$slider.trigger('unslick', [_, fromBreakpoint]);
        _.destroy();

    };

    Slick.prototype.updateArrows = function() {

        var _ = this,
            centerOffset;

        centerOffset = Math.floor(_.options.slidesToShow / 2);

        if ( _.options.arrows === true &&
            _.slideCount > _.options.slidesToShow &&
            !_.options.infinite ) {

            _.$prevArrow.removeClass('slick-disabled').attr('aria-disabled', 'false');
            _.$nextArrow.removeClass('slick-disabled').attr('aria-disabled', 'false');

            if (_.currentSlide === 0) {

                _.$prevArrow.addClass('slick-disabled').attr('aria-disabled', 'true');
                _.$nextArrow.removeClass('slick-disabled').attr('aria-disabled', 'false');

            } else if (_.currentSlide >= _.slideCount - _.options.slidesToShow && _.options.centerMode === false) {

                _.$nextArrow.addClass('slick-disabled').attr('aria-disabled', 'true');
                _.$prevArrow.removeClass('slick-disabled').attr('aria-disabled', 'false');

            } else if (_.currentSlide >= _.slideCount - 1 && _.options.centerMode === true) {

                _.$nextArrow.addClass('slick-disabled').attr('aria-disabled', 'true');
                _.$prevArrow.removeClass('slick-disabled').attr('aria-disabled', 'false');

            }

        }

    };

    Slick.prototype.updateDots = function() {

        var _ = this;

        if (_.$dots !== null) {

            _.$dots
                .find('li')
                    .removeClass('slick-active')
                    .end();

            _.$dots
                .find('li')
                .eq(Math.floor(_.currentSlide / _.options.slidesToScroll))
                .addClass('slick-active');

        }

    };

    Slick.prototype.visibility = function() {

        var _ = this;

        if ( _.options.autoplay ) {

            if ( document[_.hidden] ) {

                _.interrupted = true;

            } else {

                _.interrupted = false;

            }

        }

    };

    $.fn.slick = function() {
        var _ = this,
            opt = arguments[0],
            args = Array.prototype.slice.call(arguments, 1),
            l = _.length,
            i,
            ret;
        for (i = 0; i < l; i++) {
            if (typeof opt == 'object' || typeof opt == 'undefined')
                _[i].slick = new Slick(_[i], opt);
            else
                ret = _[i].slick[opt].apply(_[i].slick, args);
            if (typeof ret != 'undefined') return ret;
        }
        return _;
    };

}));
;!function(root, factory) {
    "function" == typeof define && define.amd ? // AMD. Register as an anonymous module unless amdModuleId is set
    define([], function() {
        return root.svg4everybody = factory();
    }) : "object" == typeof module && module.exports ? // Node. Does not work with strict CommonJS, but
    // only CommonJS-like environments that support module.exports,
    // like Node.
    module.exports = factory() : root.svg4everybody = factory();
}(this, function() {
    /*! svg4everybody v2.1.9 | github.com/jonathantneal/svg4everybody */
    function embed(parent, svg, target) {
        // if the target exists
        if (target) {
            // create a document fragment to hold the contents of the target
            var fragment = document.createDocumentFragment(), viewBox = !svg.hasAttribute("viewBox") && target.getAttribute("viewBox");
            // conditionally set the viewBox on the svg
            viewBox && svg.setAttribute("viewBox", viewBox);
            // copy the contents of the clone into the fragment
            for (// clone the target
            var clone = target.cloneNode(!0); clone.childNodes.length; ) {
                fragment.appendChild(clone.firstChild);
            }
            // append the fragment into the svg
            parent.appendChild(fragment);
        }
    }
    function loadreadystatechange(xhr) {
        // listen to changes in the request
        xhr.onreadystatechange = function() {
            // if the request is ready
            if (4 === xhr.readyState) {
                // get the cached html document
                var cachedDocument = xhr._cachedDocument;
                // ensure the cached html document based on the xhr response
                cachedDocument || (cachedDocument = xhr._cachedDocument = document.implementation.createHTMLDocument(""), 
                cachedDocument.body.innerHTML = xhr.responseText, xhr._cachedTarget = {}), // clear the xhr embeds list and embed each item
                xhr._embeds.splice(0).map(function(item) {
                    // get the cached target
                    var target = xhr._cachedTarget[item.id];
                    // ensure the cached target
                    target || (target = xhr._cachedTarget[item.id] = cachedDocument.getElementById(item.id)), 
                    // embed the target into the svg
                    embed(item.parent, item.svg, target);
                });
            }
        }, // test the ready state change immediately
        xhr.onreadystatechange();
    }
    function svg4everybody(rawopts) {
        function oninterval() {
            // while the index exists in the live <use> collection
            for (// get the cached <use> index
            var index = 0; index < uses.length; ) {
                // get the current <use>
                var use = uses[index], parent = use.parentNode, svg = getSVGAncestor(parent), src = use.getAttribute("xlink:href") || use.getAttribute("href");
                if (!src && opts.attributeName && (src = use.getAttribute(opts.attributeName)), 
                svg && src) {
                    if (polyfill) {
                        if (!opts.validate || opts.validate(src, svg, use)) {
                            // remove the <use> element
                            parent.removeChild(use);
                            // parse the src and get the url and id
                            var srcSplit = src.split("#"), url = srcSplit.shift(), id = srcSplit.join("#");
                            // if the link is external
                            if (url.length) {
                                // get the cached xhr request
                                var xhr = requests[url];
                                // ensure the xhr request exists
                                xhr || (xhr = requests[url] = new XMLHttpRequest(), xhr.open("GET", url), xhr.send(), 
                                xhr._embeds = []), // add the svg and id as an item to the xhr embeds list
                                xhr._embeds.push({
                                    parent: parent,
                                    svg: svg,
                                    id: id
                                }), // prepare the xhr ready state change event
                                loadreadystatechange(xhr);
                            } else {
                                // embed the local id into the svg
                                embed(parent, svg, document.getElementById(id));
                            }
                        } else {
                            // increase the index when the previous value was not "valid"
                            ++index, ++numberOfSvgUseElementsToBypass;
                        }
                    }
                } else {
                    // increase the index when the previous value was not "valid"
                    ++index;
                }
            }
            // continue the interval
            (!uses.length || uses.length - numberOfSvgUseElementsToBypass > 0) && requestAnimationFrame(oninterval, 67);
        }
        var polyfill, opts = Object(rawopts), newerIEUA = /\bTrident\/[567]\b|\bMSIE (?:9|10)\.0\b/, webkitUA = /\bAppleWebKit\/(\d+)\b/, olderEdgeUA = /\bEdge\/12\.(\d+)\b/, edgeUA = /\bEdge\/.(\d+)\b/, inIframe = window.top !== window.self;
        polyfill = "polyfill" in opts ? opts.polyfill : newerIEUA.test(navigator.userAgent) || (navigator.userAgent.match(olderEdgeUA) || [])[1] < 10547 || (navigator.userAgent.match(webkitUA) || [])[1] < 537 || edgeUA.test(navigator.userAgent) && inIframe;
        // create xhr requests object
        var requests = {}, requestAnimationFrame = window.requestAnimationFrame || setTimeout, uses = document.getElementsByTagName("use"), numberOfSvgUseElementsToBypass = 0;
        // conditionally start the interval if the polyfill is active
        polyfill && oninterval();
    }
    function getSVGAncestor(node) {
        for (var svg = node; "svg" !== svg.nodeName.toLowerCase() && (svg = svg.parentNode); ) {}
        return svg;
    }
    return svg4everybody;
});;!function(a,b){"function"==typeof define&&define.amd?
// AMD. Register as an anonymous module unless amdModuleId is set
define([],function(){return a.svg4everybody=b()}):"object"==typeof exports?module.exports=b():a.svg4everybody=b()}(this,function(){/*! svg4everybody v2.0.3 | github.com/jonathantneal/svg4everybody */
function a(a,b){
// if the target exists
if(b){
// create a document fragment to hold the contents of the target
var c=document.createDocumentFragment(),d=!a.getAttribute("viewBox")&&b.getAttribute("viewBox");
// conditionally set the viewBox on the svg
d&&a.setAttribute("viewBox",d);
// copy the contents of the clone into the fragment
for(
// clone the target
var e=b.cloneNode(!0);e.childNodes.length;)c.appendChild(e.firstChild);
// append the fragment into the svg
a.appendChild(c)}}function b(b){
// listen to changes in the request
b.onreadystatechange=function(){
// if the request is ready
if(4===b.readyState){
// get the cached html document
var c=b._cachedDocument;
// ensure the cached html document based on the xhr response
c||(c=b._cachedDocument=document.implementation.createHTMLDocument(""),c.body.innerHTML=b.responseText,b._cachedTarget={}),
// clear the xhr embeds list and embed each item
b._embeds.splice(0).map(function(d){
// get the cached target
var e=b._cachedTarget[d.id];
// ensure the cached target
e||(e=b._cachedTarget[d.id]=c.getElementById(d.id)),
// embed the target into the svg
a(d.svg,e)})}},
// test the ready state change immediately
b.onreadystatechange()}function c(c){function d(){
// while the index exists in the live <use> collection
for(
// get the cached <use> index
var c=0;c<l.length;){
// get the current <use>
var g=l[c],h=g.parentNode;if(h&&/svg/i.test(h.nodeName)){var i=g.getAttribute("xlink:href");if(e&&(!f.validate||f.validate(i,h,g))){
// remove the <use> element
h.removeChild(g);
// parse the src and get the url and id
var m=i.split("#"),n=m.shift(),o=m.join("#");
// if the link is external
if(n.length){
// get the cached xhr request
var p=j[n];
// ensure the xhr request exists
p||(p=j[n]=new XMLHttpRequest,p.open("GET",n),p.send(),p._embeds=[]),
// add the svg and id as an item to the xhr embeds list
p._embeds.push({svg:h,id:o}),
// prepare the xhr ready state change event
b(p)}else
// embed the local id into the svg
a(h,document.getElementById(o))}}else
// increase the index when the previous value was not "valid"
++c}
// continue the interval
k(d,67)}var e,f=Object(c),g=/\bTrident\/[567]\b|\bMSIE (?:9|10)\.0\b/,h=/\bAppleWebKit\/(\d+)\b/,i=/\bEdge\/12\.(\d+)\b/;e="polyfill"in f?f.polyfill:g.test(navigator.userAgent)||(navigator.userAgent.match(i)||[])[1]<10547||(navigator.userAgent.match(h)||[])[1]<537;
// create xhr requests object
var j={},k=window.requestAnimationFrame||setTimeout,l=document.getElementsByTagName("use");
// conditionally start the interval if the polyfill is active
e&&d()}return c});