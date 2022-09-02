const timer = ms => new Promise(res => setTimeout(res, ms));
var skipped = false;

function skipText() {
  console.log("vriska")
  skipped = true;
}

async function flareType() {
  var messages = document.querySelectorAll("tp");
  var msgTexts = [];
  skipped = false;
  messages.forEach((msg) => {
    //get texts
    msgTexts.push(msg.innerHTML)
    msg.innerHTML = "‎‏‏‎ ‎"
    
    //load sounds
    var dummySound = getComputedStyle(msg).getPropertyValue('--sound');
    var dummyAudio = new Audio(dummySound);
  });
  //wait for audio to load
  await timer(1000);
  //begin writing
  for (let i = 0; i < msgTexts.length; i++) {
    //clear line
    messages[i].innerHTML = ""
    //get speed
    var msgSpeed = messages[i].getAttribute("s") || 50;
    //get sound
    var msgSound = getComputedStyle(messages[i]).getPropertyValue('--sound');
    var msgAudioA = new Audio(msgSound);
    var msgAudioB = new Audio(msgSound);
    var msgAudioC = new Audio(msgSound);
    
    //write text
    var msgString = "";
    for (let j = 0; j < msgTexts[i].length; j++) {
      var chara = msgTexts[i].charAt(j);
      msgString += chara;
      messages[i].innerText = msgString
      if (msgSound && chara !== " " && !skipped) {
        if (j%3 == 1) {
          msgAudioA.currentTime = 0;
          msgAudioA.play();
        } else if (j%3 == 2) {
          msgAudioB.currentTime = 0;
          msgAudioB.play();
        } else {
          msgAudioC.currentTime = 0;
          msgAudioC.play();
        }
      };
      
      if (skipped) {msgSpeed = 0}
      await timer(msgSpeed);
    };
    //get break time
    var msgBreak = messages[i].getAttribute("b") || 1000;
    if (skipped) {msgBreak = 0}
    //break
    await timer(msgBreak);
  };
}

document.querySelector('#content').setAttribute("onclick", "skipText()")

MSPFA.slide.push(() => {
  flareType()
});

if (MSPFA.story.i != 43399) {
  flareType()
}